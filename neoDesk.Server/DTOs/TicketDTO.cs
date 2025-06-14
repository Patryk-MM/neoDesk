namespace neoDesk.Server.DTOs;

public class TicketDTO {
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string CreatedAt { get; set; }
    public required string Category { get; set; }
    public required string Status { get; set; }
}