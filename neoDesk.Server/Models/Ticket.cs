namespace neoDesk.Server.Models;

public class Ticket {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Category Category { get; set; }
    public Status Status { get; set; }
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