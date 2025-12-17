using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using neoDesk.Server.Data;
using neoDesk.Server.DTOs;
using neoDesk.Server.Models;
using System.Security.Claims;

namespace neoDesk.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all endpoints
public class TicketController : ControllerBase
{
    private readonly NeoDeskDbContext _context;

    public TicketController(NeoDeskDbContext context)
    {
        _context = context;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }

    // GET api/ticket
    [HttpGet(Name = "GetTickets")]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> Get()
    {
        var tickets = await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        var ticketDTOs = tickets.Select(t => new TicketDTO
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            CreatedAt = t.CreatedAt.ToString("g"),
            Category = t.Category,
            Status = t.Status,
            CreatedBy = t.CreatedByUser.Name,
            AssignedTo = t.AssignedToUser?.Name
        });

        return Ok(ticketDTOs);
    }

    // GET api/ticket/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDTO>> Get(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
        {
            return NotFound();
        }

        var ticketDTO = new TicketDTO
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedAt = ticket.CreatedAt.ToString("g"),
            Category = ticket.Category,
            Status = ticket.Status,
            CreatedBy = ticket.CreatedByUser.Name,
            AssignedTo = ticket.AssignedToUser?.Name
        };

        return Ok(ticketDTO);
    }

    // POST api/ticket
    [HttpPost]
    public async Task<ActionResult<TicketDTO>> Post([FromBody] CreateTicketDTO createTicketDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentUserId = GetCurrentUserId();
        if (currentUserId == 0)
        {
            return Unauthorized();
        }

        var ticket = new Ticket
        {
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

        var ticketDTO = new TicketDTO
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedAt = ticket.CreatedAt.ToString("g"),
            Category = ticket.Category,
            Status = ticket.Status,
            CreatedBy = ticket.CreatedByUser.Name,
            AssignedTo = null // No assignment on creation
        };

        return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticketDTO);
    }

    // PUT api/ticket/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateTicketDTO updateTicketDTO)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        var currentUserId = GetCurrentUserId();
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        
        // Check permissions: only creator or admin can edit
        // Technicians can edit if ticket is assigned to them
        bool canEdit = false;
        
        if (userRole == "Admin")
        {
            canEdit = true; // Admins can edit any ticket
        }
        else if (ticket.CreatedByUserId == currentUserId)
        {
            canEdit = true; // Creator can edit their own ticket
        }
        else if (userRole == "Technician" && ticket.AssignedToUserId == currentUserId)
        {
            canEdit = true; // Assigned technician can edit
        }

        if (!canEdit)
        {
            return Forbid("Nie masz uprawnień do edycji tego zgłoszenia. Tylko utworzyciel zgłoszenia, przypisany technik lub administrator może je edytować.");
        }

        // Update basic fields
        ticket.Title = updateTicketDTO.Title;
        ticket.Description = updateTicketDTO.Description;
        ticket.Category = updateTicketDTO.Category;
        ticket.Status = updateTicketDTO.Status;
        ticket.UpdatedAt = DateTime.Now;

        // Only admins and technicians can assign tickets
        if (userRole == "Admin" || userRole == "Technician")
        {
            ticket.AssignedToUserId = updateTicketDTO.AssignedToUserId;
        }

        // Only admins can change creation date
        if (userRole == "Admin" && updateTicketDTO.CreatedAt.HasValue)
        {
            ticket.CreatedAt = updateTicketDTO.CreatedAt.Value;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST api/ticket/5/assign - Separate endpoint for assignment
    [HttpPost("{id}/assign")]
    [Authorize(Roles = "Admin,Technician")]
    public async Task<IActionResult> AssignTicket(int id, [FromBody] AssignTicketDTO assignTicketDTO)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        // Validate assigned user exists if provided
        if (assignTicketDTO.AssignedToUserId.HasValue)
        {
            var assignedUser = await _context.Users.FindAsync(assignTicketDTO.AssignedToUserId.Value);
            if (assignedUser == null || !assignedUser.IsActive)
            {
                return BadRequest("Wybrany użytkownik nie istnieje lub jest nieaktywny");
            }

            // Only allow assignment to Technicians or Admins
            if (assignedUser.Role == UserRole.EndUser)
            {
                return BadRequest("Można przypisać zgłoszenie tylko do technika lub administratora");
            }
        }

        ticket.AssignedToUserId = assignTicketDTO.AssignedToUserId;
        ticket.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(new { message = assignTicketDTO.AssignedToUserId.HasValue ? "Zgłoszenie zostało przypisane" : "Przypisanie zgłoszenia zostało usunięte" });
    }

    // PUT api/ticket/5/status - Quick status update
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TicketStatusUpdateDTO statusUpdateDTO)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        var currentUserId = GetCurrentUserId();
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        
        // Check permissions for status updates
        bool canUpdateStatus = false;
        
        if (userRole == "Admin")
        {
            canUpdateStatus = true;
        }
        else if (userRole == "Technician" && ticket.AssignedToUserId == currentUserId)
        {
            canUpdateStatus = true;
        }
        else if (ticket.CreatedByUserId == currentUserId && 
                 (statusUpdateDTO.Status == Status.New || statusUpdateDTO.Status == Status.Suspended))
        {
            // Users can only reopen or suspend their own tickets
            canUpdateStatus = true;
        }

        if (!canUpdateStatus)
        {
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
    public async Task<IActionResult> Delete(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET api/ticket/my-tickets
    [HttpGet("my-tickets")]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> GetMyTickets()
    {
        var currentUserId = GetCurrentUserId();
        
        var tickets = await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .Where(t => t.CreatedByUserId == currentUserId || t.AssignedToUserId == currentUserId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        var ticketDTOs = tickets.Select(t => new TicketDTO
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            CreatedAt = t.CreatedAt.ToString("g"),
            Category = t.Category,
            Status = t.Status,
            CreatedBy = t.CreatedByUser.Name,
            AssignedTo = t.AssignedToUser?.Name
        });

        return Ok(ticketDTOs);
    }
}