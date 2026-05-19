using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Data;
using ServiceDesk.Api.Models;
using ServiceDesk.Api.Services;

namespace ServiceDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITicketService _ticketService;
    public TicketsController(AppDbContext context, ITicketService ticketService)
    {
        _context = context;
        _ticketService = ticketService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _context.Tickets
            .Include(t => t.AssignedTo)
            .ToListAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) return NotFound();
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest request)
    {
        var ticket = new Ticket
        {
            Title = request.Title,
            Description = request.Description
        };

        var assignee = await _ticketService.FindAvailableAssigneeAsync();
        if (assignee == null) return BadRequest("No available team members to assign the ticket to.");
            
        ticket.AssignedTo = assignee;

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound();

        ticket.Status = request.Status;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound();

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

public record CreateTicketRequest(string Title, string Description);
public record UpdateStatusRequest(TicketStatus Status);
