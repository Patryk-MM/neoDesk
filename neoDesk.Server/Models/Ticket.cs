using System.ComponentModel.DataAnnotations;

namespace neoDesk.Server.Models;

public class Ticket
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    
    public Category Category { get; set; }
    public Status Status { get; set; }

    public ICollection<Comment> Comments { get; set; }
    
    // Foreign Keys
    public int CreatedByUserId { get; set; }
    public int? AssignedToUserId { get; set; } = null;
    
    // Navigation Properties
    public User CreatedByUser { get; set; } = null!;
    public User? AssignedToUser { get; set; }
}

public enum Status {
    New,
    Assigned,
    Suspended,
    Solved
}

public enum Category {
    Software,
    Hardware
}