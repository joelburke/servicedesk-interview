namespace ServiceDesk.Api.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public int? AssignedToId { get; set; }
    public TeamMember? AssignedTo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum TicketStatus
{
    Open,
    InProgress,
    Resolved,
    Closed
}
