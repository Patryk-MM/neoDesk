using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using neoDesk.Server.Data;
using neoDesk.Server.DTOs;
using neoDesk.Server.DTOs.Auth;
using neoDesk.Server.Models;
using BCrypt.Net;

namespace neoDesk.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly NeoDeskDbContext _context;

    public UsersController(NeoDeskDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        var users = await _context.Users
            .OrderBy(u => u.Name)
            .ToListAsync();

        var userDtos = users.Select(MapToUserDTO);
        return Ok(userDtos);
    }

    [HttpGet("assignable")]
    [Authorize(Roles = "Admin,Technician")] 
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAssignableUsers()
    {
        var users = await _context.Users
            .Where(u => u.IsActive && (u.Role == UserRole.Technician || u.Role == UserRole.Admin))
            .OrderBy(u => u.Name)
            .ToListAsync();

        var userDtos = users.Select(MapToUserDTO);
        return Ok(userDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(MapToUserDTO(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUserDTO createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _context.Users.AnyAsync(u => u.Email.ToLower() == createUserDto.Email.ToLower()))
        {
            return BadRequest(new { message = "Użytkownik o podanym adresie email już istnieje" });
        }

        if (!Enum.TryParse<UserRole>(createUserDto.Role, out var userRole))
        {
            return BadRequest(new { message = "Nieprawidłowa rola użytkownika" });
        }

        var user = new User
        {
            Name = createUserDto.Name,
            Email = createUserDto.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
            Role = userRole,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, MapToUserDTO(user));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromBody] UpdateUserDTO updateUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (await _context.Users.AnyAsync(u => u.Id != id && u.Email.ToLower() == updateUserDto.Email.ToLower()))
        {
            return BadRequest(new { message = "Użytkownik o podanym adresie email już istnieje" });
        }

        if (!Enum.TryParse<UserRole>(updateUserDto.Role, out var userRole))
        {
            return BadRequest(new { message = "Nieprawidłowa rola użytkownika" });
        }

        user.Name = updateUserDto.Name;
        user.Email = updateUserDto.Email.ToLower();
        user.Role = userRole;
        user.IsActive = updateUserDto.IsActive;

        if (!string.IsNullOrEmpty(updateUserDto.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
        }

        await _context.SaveChangesAsync();

        return Ok(MapToUserDTO(user));
    }

    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordDTO resetPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.Password);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Hasło zostało zresetowane pomyślnie" });
    }

    [HttpPost("{id}/toggle-status")]
    public async Task<ActionResult<UserDTO>> ToggleUserStatus(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.IsActive = !user.IsActive;
        await _context.SaveChangesAsync();

        return Ok(MapToUserDTO(user));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var hasTickets = await _context.Tickets.AnyAsync(t => t.CreatedByUserId == id || t.AssignedToUserId == id);
        if (hasTickets)
        {
            return BadRequest(new { message = "Nie można usunąć użytkownika, który ma przypisane zgłoszenia. Dezaktywuj konto zamiast tego." });
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static UserDTO MapToUserDTO(User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}