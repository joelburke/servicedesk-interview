using ServiceDesk.Api.Models;

namespace ServiceDesk.Api.Services;

public interface ITicketService
{
    Task<TeamMember?> FindAvailableAssigneeAsync();
}
