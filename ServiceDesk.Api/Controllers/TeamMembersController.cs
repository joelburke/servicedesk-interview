using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Data;
using ServiceDesk.Api.DTOs;

namespace ServiceDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamMembersController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var members = await context.TeamMembers
            .Select(m => new TeamMemberDto(
                m.Id, m.Name, m.MaxCapacity,
                m.Tickets.Select(t => new TicketSummaryDto(t.Id, t.Title, t.Status))))
            .ToListAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var member = await context.TeamMembers
            .Where(m => m.Id == id)
            .Select(m => new TeamMemberDto(
                m.Id, m.Name, m.MaxCapacity,
                m.Tickets.Select(t => new TicketSummaryDto(t.Id, t.Title, t.Status))))
            .FirstOrDefaultAsync();

        if (member == null) return NotFound();
        return Ok(member);
    }
}
