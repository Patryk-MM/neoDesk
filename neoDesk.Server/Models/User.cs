using System.ComponentModel.DataAnnotations;

namespace neoDesk.Server.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public UserRole Role { get; set; } = UserRole.EndUser;
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation properties
    public List<Ticket> CreatedTickets { get; set; } = new();
    public List<Ticket> AssignedTickets { get; set; } = new();
}

public enum UserRole
{
    EndUser = 0,
    Technician = 1,
    Admin = 2
}