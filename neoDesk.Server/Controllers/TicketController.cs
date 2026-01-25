using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using neoDesk.Server.Data;
using neoDesk.Server.DTOs;
using neoDesk.Server.Models;
using System.Security.Claims;
using neoDesk.Server.Helpers;

namespace neoDesk.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all endpoints
public class TicketController : ControllerBase {
    private readonly NeoDeskDbContext _context;

    public TicketController(NeoDeskDbContext context) {
        _context = context;
    }

    private int GetCurrentUserId() {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }

    private string? GetRole() {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        return userRole;
    }

    // GET api/ticket
    [HttpGet(Name = "GetTickets")]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> Get([FromQuery] TicketFilterParams filters) {
        var query = _context.Tickets.AsQueryable();
        var currentUserId = GetCurrentUserId();

        if (GetRole() == "EndUser" || GetRole() == "Technician") {
            query = query.Where(t => t.CreatedByUserId == currentUserId || t.AssignedToUserId == currentUserId);
        }

        if (!string.IsNullOrWhiteSpace(filters.SearchTerm)) {
            query = query.Where(t => t.Title.Contains(filters.SearchTerm) || t.Description.Contains(filters.SearchTerm));
        }

        if (filters.Statuses != null && filters.Statuses.Any()) {
            query = query.Where(t => filters.Statuses.Contains(t.Status));
        }

        if (filters.Categories != null && filters.Categories.Any()) {
            query = query.Where(t => filters.Categories.Contains(t.Category));        
        }

        var result = await query.Select(t => new TicketDTO {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            CreatedAt = t.CreatedAt,
            Category = t.Category,
            Status = t.Status,
            CreatedBy = new SimpleUserDTO {
                Id = t.CreatedByUserId,
                Name = t.CreatedByUser.Name,
            },
            AssignedTo = new SimpleUserDTO {
                Id = t.AssignedToUserId,
                Name = t.AssignedToUser != null ? t.AssignedToUser.Name : null
            }
        }).ToListAsync();

        return Ok(result);
    }

    // GET api/ticket/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDTO>> Get(int id) {
        var ticket = await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) {
            return NotFound();
        }

        var ticketDTO = new TicketDTO {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedAt = ticket.CreatedAt,
            Category = ticket.Category,
            Status = ticket.Status,
            CreatedBy = new SimpleUserDTO {
                Id = ticket.CreatedByUserId,
                Name = ticket.CreatedByUser.Name,

            },
            AssignedTo = new SimpleUserDTO {
                Id = ticket.AssignedToUserId,
                Name = ticket.AssignedToUser?.Name
,
            }
        };

        return Ok(ticketDTO);
    }

    // POST api/ticket
    [HttpPost]
    public async Task<ActionResult<TicketDTO>> Post([FromBody] CreateTicketDTO createTicketDTO) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var currentUserId = GetCurrentUserId();
        if (currentUserId == 0) {
            return Unauthorized();
        }

        var ticket = new Ticket {
            Title = createTicketDTO.Title,
            Description = createTicketDTO.Description,
            Category = createTicketDTO.Category,
            Status = createTicketDTO.Status,
            CreatedByUserId = currentUserId, // Use actual logged-in user
                                             // AssignedToUserId is null by default - use separate assign endpoint
            CreatedAt = DateTime.Now
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        // Reload with user data
        await _context.Entry(ticket)
            .Reference(t => t.CreatedByUser)
            .LoadAsync();

        var ticketDTO = new TicketDTO {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedAt = ticket.CreatedAt,
            Category = ticket.Category,
            Status = ticket.Status,
            CreatedBy = new SimpleUserDTO {
                Id = ticket.CreatedByUserId,
                Name = ticket.CreatedByUser.Name,

            },
            AssignedTo = new SimpleUserDTO {
                Id = ticket.AssignedToUserId,
                Name = ticket.AssignedToUser?.Name
,
            }
        };

        return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticketDTO);
    }

    // PUT api/ticket/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateTicketDTO updateTicketDTO) {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) {
            return NotFound();
        }

        var currentUserId = GetCurrentUserId();
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Check permissions: only creator or admin can edit
        // Technicians can edit if ticket is assigned to them
        bool canEdit = false;

        if (userRole == "Admin") {
            canEdit = true; // Admins can edit any ticket
        }
        else if (ticket.CreatedByUserId == currentUserId) {
            canEdit = true; // Creator can edit their own ticket
        }
        else if (userRole == "Technician" && ticket.AssignedToUserId == currentUserId) {
            canEdit = true; // Assigned technician can edit
        }

        if (!canEdit) {
            return Forbid("Nie masz uprawnień do edycji tego zgłoszenia. Tylko utworzyciel zgłoszenia, przypisany technik lub administrator może je edytować.");
        }

        // Update basic fields
        ticket.Title = updateTicketDTO.Title;
        ticket.Description = updateTicketDTO.Description;
        ticket.Category = updateTicketDTO.Category;
        ticket.Status = updateTicketDTO.Status;
        ticket.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}/assign")]
    [Authorize(Roles = "Admin,Technician")]
    public async Task<IActionResult> AssignTicket(int id, [FromBody] int? assignedToUserId) {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) {
            return NotFound();
        }

        // POPRAWKA: Waliduj Usera tylko jeśli przysłano konkretne ID (nie null)
        if (!(assignedToUserId is null)) {
            var assignedUser = await _context.Users.FindAsync(assignedToUserId.Value);

            if (assignedUser is null || !assignedUser.IsActive) {
                return BadRequest("Wybrany użytkownik nie istnieje lub jest nieaktywny");
            }

            if (assignedUser.Role == UserRole.EndUser) {
                return BadRequest("Można przypisać zgłoszenie tylko do technika lub administratora");
            }
        }

        // Jeśli assignedToUserId jest null, kod przechodzi tutaj i wpisuje null do bazy
        ticket.AssignedToUserId = assignedToUserId;
        ticket.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        // POPRAWKA 2: Naprawiona logika wiadomości (były odwrócone)
        var message = !(assignedToUserId is null)
            ? "Zgłoszenie zostało przypisane"
            : "Przypisanie zgłoszenia zostało usunięte";

        return Ok(new { message });
    }


    // PUT api/ticket/5/status - Quick status update
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TicketStatusUpdateDTO statusUpdateDTO) {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) {
            return NotFound();
        }

        var currentUserId = GetCurrentUserId();
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Check permissions for status updates
        bool canUpdateStatus = false;

        if (userRole == "Admin") {
            canUpdateStatus = true;
        }
        else if (userRole == "Technician" && ticket.AssignedToUserId == currentUserId) {
            canUpdateStatus = true;
        }
        else if (ticket.CreatedByUserId == currentUserId &&
                 (statusUpdateDTO.Status == Status.New || statusUpdateDTO.Status == Status.Suspended)) {
            // Users can only reopen or suspend their own tickets
            canUpdateStatus = true;
        }

        if (!canUpdateStatus) {
            return Forbid("Nie masz uprawnień do zmiany statusu tego zgłoszenia");
        }

        ticket.Status = statusUpdateDTO.Status;
        ticket.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Status zgłoszenia został zaktualizowany" });
    }

    // DELETE api/ticket/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Only admins can delete
    public async Task<IActionResult> Delete(int id) {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) {
            return NotFound();
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET api/ticket/my-tickets
    [HttpGet("my-tickets")]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> GetMyTickets() {
        var currentUserId = GetCurrentUserId();

        var tickets = await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .Where(t => t.CreatedByUserId == currentUserId || t.AssignedToUserId == currentUserId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        var ticketDTOs = tickets.Select(t => new TicketDTO {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            CreatedAt = t.CreatedAt,
            Category = t.Category,
            Status = t.Status,
            CreatedBy = new SimpleUserDTO {
                Id = t.CreatedByUserId,
                Name = t.CreatedByUser.Name,

            },
            AssignedTo = new SimpleUserDTO {
                Id = t.AssignedToUserId,
                Name = t.AssignedToUser?.Name
,
            }
        });

        return Ok(ticketDTOs);
    }
}