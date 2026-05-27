using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Data;
using ServiceDesk.Api.DTOs;
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
            .Select(t => new TicketDto(
                t.Id, t.Title, t.Description, t.Status, t.AssignedToId,
                t.AssignedTo == null ? null : new TeamMemberSummaryDto(t.AssignedTo.Id, t.AssignedTo.Name),
                t.CreatedAt))
            .ToListAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _context.Tickets
            .Where(t => t.Id == id)
            .Select(t => new TicketDto(
                t.Id, t.Title, t.Description, t.Status, t.AssignedToId,
                t.AssignedTo == null ? null : new TeamMemberSummaryDto(t.AssignedTo.Id, t.AssignedTo.Name),
                t.CreatedAt))
            .FirstOrDefaultAsync();

        if (ticket == null) return NotFound();
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest request)
    {
        var ticket = await _ticketService.CreateTicketAsync(request.Title, request.Description);
        if (ticket == null) return BadRequest("No available team members to assign the ticket to.");

        var dto = ToDto(ticket);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
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

    private static TicketDto ToDto(Ticket t) => new(
        t.Id, t.Title, t.Description, t.Status, t.AssignedToId,
        t.AssignedTo is null ? null : new TeamMemberSummaryDto(t.AssignedTo.Id, t.AssignedTo.Name),
        t.CreatedAt);
}

public record CreateTicketRequest(string Title, string Description);
public record UpdateStatusRequest(TicketStatus Status);
