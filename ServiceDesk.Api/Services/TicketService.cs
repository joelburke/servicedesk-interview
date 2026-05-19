using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Data;
using ServiceDesk.Api.Models;

namespace ServiceDesk.Api.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _context;

    public TicketService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TeamMember?> FindAvailableAssigneeAsync()
    {
        var result =  _context.TeamMembers.Select(tm => new
        {
            TeamMember = tm,
            TicketCount = tm.Tickets.Count
        })
        .Where(x => x.TicketCount < x.TeamMember.MaxCapacity)
        .OrderBy(x => x.TicketCount);
        
        return await Task.FromResult(result.FirstOrDefault()?.TeamMember);
    }
}
