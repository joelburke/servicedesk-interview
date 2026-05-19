using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Data;
using ServiceDesk.Api.Services;

namespace ServiceDesk.Tests;

public class TicketServiceTests
{
    private AppDbContext _context;
    private static string ALICE = "Alice";
    private static string BOB = "Bob";
    private static string CAROL = "Carol";

    public TicketServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void TicketService_FindAvailableAssigneeAsync_ShouldReturnNotNull()
    {
        // Arrange
        var ticketService = new TicketService(_context);

        // Act
        var result = ticketService.FindAvailableAssigneeAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task TicketService_FindAvailableAssigneeAsync_ShouldReturnTeamMember()
    {
        // Arrange
        var ticketService = new TicketService(_context);

        // Act
        var result = ticketService.FindAvailableAssigneeAsync();
        var teamMember = await result;
        
        // Assert
        Assert.Equal("Alice", teamMember?.Name);
    }

    
    [Fact]
    public async Task TicketService_FindAvailableAssigneeAsync_ShouldReturnTeamMemberWithLowestTickets()
    {
        // Arrange
        var ticketService = new TicketService(_context);

        // Act
        var result = ticketService.FindAvailableAssigneeAsync();
        var teamMember = await result;
        
        // Assert
        Assert.Equal(ALICE, teamMember?.Name);
    }
}
