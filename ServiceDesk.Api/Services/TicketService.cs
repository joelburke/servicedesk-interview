using ServiceDesk.Api.Models;

namespace ServiceDesk.Api.Services;

public class TicketService : ITicketService
{
    public Task<TeamMember?> FindAvailableAssigneeAsync()
    {
        throw new NotImplementedException();
    }
}
