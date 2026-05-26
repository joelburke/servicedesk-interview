using ServiceDesk.Api.Models;

namespace ServiceDesk.Api.Services;

public interface ITicketService
{
    Task<TeamMember?> FindFirstAvailableAsigneeNotAtTicketCapacityAsync();
    Task<Ticket?> CreateTicketAsync(string title, string description);
}
