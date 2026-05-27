using ServiceDesk.Api.Models;

namespace ServiceDesk.Api.DTOs;

public record TeamMemberSummaryDto(int Id, string Name);

public record TicketDto(
    int Id,
    string Title,
    string Description,
    TicketStatus Status,
    int? AssignedToId,
    TeamMemberSummaryDto? AssignedTo,
    DateTime CreatedAt);

public record TicketSummaryDto(int Id, string Title, TicketStatus Status);

public record TeamMemberDto(
    int Id,
    string Name,
    int MaxCapacity,
    IEnumerable<TicketSummaryDto> Tickets);
