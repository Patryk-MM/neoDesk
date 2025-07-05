using System.ComponentModel.DataAnnotations;
using neoDesk.Server.Models;

namespace neoDesk.Server.DTOs;

public class CreateTicketDTO
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public Category Category { get; set; }
    public Status Status { get; set; } = Status.New;
    
    // Optional creation date - if not provided, will use current time
    public DateTime? CreatedAt { get; set; }
}

public class TicketDTO
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string CreatedAt { get; set; }
    public required string Category { get; set; }
    public required string Status { get; set; }
    public string? CreatedBy { get; set; }
    public string? AssignedTo { get; set; }
}

public class UpdateTicketDTO
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public Category Category { get; set; }
    public Status Status { get; set; }
    
    // Optional assignment to user
    public int? AssignedToUserId { get; set; }
    
    // Optional creation date update (usually only admins should change this)
    public DateTime? CreatedAt { get; set; }
}

public class AssignTicketDTO
{
    [Required]
    public int TicketId { get; set; }
    
    public int? AssignedToUserId { get; set; } // null to unassign
}


public class TicketStatusUpdateDTO
{
    [Required]
    public Status Status { get; set; }
    
    public string? Comment { get; set; } // Optional comment for status change
}