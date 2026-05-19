using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Data;

namespace ServiceDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamMembersController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var members = await context.TeamMembers
            .Include(m => m.Tickets)
            .ToListAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var member = await context.TeamMembers
            .Include(m => m.Tickets)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (member == null) return NotFound();
        return Ok(member);
    }
}
