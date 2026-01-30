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
}

public class TicketDTO
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Category Category { get; set; }
    public required Status Status { get; set; }
    public required IEnumerable<CommentDTO> Comments { get; set; }
    public required SimpleUserDTO CreatedBy { get; set; }
    public required SimpleUserDTO AssignedTo { get; set; }
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
}

public class AssignTicketDTO
{
    public int? AssignedToUserId { get; set; } // null to unassign
}


public class TicketStatusUpdateDTO
{
    [Required]
    public Status Status { get; set; }
    
    public string? Comment { get; set; } // Optional comment for status change
}