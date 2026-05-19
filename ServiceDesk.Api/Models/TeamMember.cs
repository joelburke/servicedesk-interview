namespace ServiceDesk.Api.Models;

public class TeamMember
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MaxCapacity { get; set; } = 5;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
