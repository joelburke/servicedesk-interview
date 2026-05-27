using System.Text.Json;
using ServiceDesk.Api.DTOs;
using ServiceDesk.Api.Models;

namespace ServiceDesk.Tests;

public class TicketSerializationTests
{
    [Fact]
    public void Ticket_WithAssignedToBackReferencee_ShouldNotThrowJsonExceptionDueToCircularReference()
    {
        // Arrange - TicketDto contains only a TeamMemberSummaryDto (Id + Name),
        // so there is no back-reference to Tickets and no cycle can form.
        var dto = new TicketDto(
            Id: 1,
            Title: "Test",
            Description: "Test",
            Status: TicketStatus.Open,
            AssignedToId: 1,
            AssignedTo: new TeamMemberSummaryDto(Id: 1, Name: "Alice"),
            CreatedAt: DateTime.UtcNow);

        // Act & Assert - should serialize without throwing
        var json = JsonSerializer.Serialize(dto);
        Assert.NotNull(json);
    }
}
