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

    public async Task<TeamMember?> FindFirstAvailableAsigneeNotAtTicketCapacityAsync()
    {
        var result =  _context.TeamMembers.Select(tm => new
        {
            TeamMember = tm,
            TicketCount = tm.Tickets.Count
        })
        .Where(x => x.TicketCount < x.TeamMember.MaxCapacity)
        .OrderBy(x => x.TicketCount);
        
        return await result.Select(x => x.TeamMember).FirstOrDefaultAsync();
    }

    public async Task<Ticket?> CreateTicketAsync(string title, string description)
    {
        var assignee = await FindFirstAvailableAsigneeNotAtTicketCapacityAsync();
        if (assignee == null) return null;

        var ticket = new Ticket
        {
            Title = title,
            Description = description,
            AssignedTo = assignee
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }
}
