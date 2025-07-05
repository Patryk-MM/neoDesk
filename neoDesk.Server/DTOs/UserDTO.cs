using System.ComponentModel.DataAnnotations;

namespace neoDesk.Server.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}


public class CreateUserDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public string Role { get; set; } = "EndUser"; // EndUser, Technician, Admin
}

public class UpdateUserDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Role { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    [MinLength(6)]
    public string? Password { get; set; }
}