using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Data;
using ServiceDesk.Api.Models;
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
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task TicketService_FindAvailableAssigneeAsync_ShouldReturnNotNull()
    {
        // Arrange
        var ticketService = new TicketService(_context);

        // Act
        var result = await ticketService.FindFirstAvailableAsigneeNotAtTicketCapacityAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task TicketService_FindAvailableAssigneeAsync_ShouldReturnTeamMember()
    {
        // Arrange
        var ticketService = new TicketService(_context);

        // Act
        var result = ticketService.FindFirstAvailableAsigneeNotAtTicketCapacityAsync();
        var teamMember = await result;
        
        // Assert
        Assert.Equal("Alice", teamMember?.Name);
    }

    
    [Fact]
    public async Task TicketService_FindAvailableAssigneeAsync_ShouldReturnTeamMemberWithLowestTickets()
    {
        // Arrange - give Alice 2 tickets so Bob (0 tickets) should win
        var alice = _context.TeamMembers.First(m => m.Name == ALICE);
        _context.Tickets.AddRange(
            new ServiceDesk.Api.Models.Ticket { Title = "T1", AssignedTo = alice },
            new ServiceDesk.Api.Models.Ticket { Title = "T2", AssignedTo = alice }
        );
        await _context.SaveChangesAsync();

        var ticketService = new TicketService(_context);

        // Act
        var teamMember = await ticketService.FindFirstAvailableAsigneeNotAtTicketCapacityAsync();

        // Assert
        Assert.Equal(BOB, teamMember?.Name);
    }

        
    [Fact]
    public async Task TicketService_FindAvailableAssigneeAsync_ShouldNotAssignToSomeoneOverCapacity()
    {
        // Arrange - give Alice 2 tickets so Bob (0 tickets) should win
        var carol = _context.TeamMembers.First(m => m.Name == CAROL);
        _context.Tickets.AddRange(
            new Ticket { Title = "T1", AssignedTo = carol },
            new Ticket { Title = "T2", AssignedTo = carol },
            new Ticket { Title = "T3", AssignedTo = carol }
        );
        var bob = _context.TeamMembers.First(m => m.Name == BOB);
        _context.Tickets.AddRange(
            new Ticket { Title = "T1", AssignedTo = bob },
            new Ticket { Title = "T2", AssignedTo = bob },
            new Ticket { Title = "T3", AssignedTo = bob }
        );
        var alice = _context.TeamMembers.First(m => m.Name == ALICE);
        _context.Tickets.AddRange(
            new Ticket { Title = "T1", AssignedTo = alice },
            new Ticket { Title = "T2", AssignedTo = alice },
            new Ticket { Title = "T3", AssignedTo = alice }
        );
        await _context.SaveChangesAsync();

        var ticketService = new TicketService(_context);

        // Act
        var teamMember = await ticketService.FindFirstAvailableAsigneeNotAtTicketCapacityAsync();

        // Assert
        Assert.Equal(ALICE, teamMember?.Name);
    }
  
    [Fact]
    public async Task TicketService_FindAvailableAssigneeAsync_ShouldNotAssignToSomeoneOverCapacity2()
    {
        // Arrange - give Alice 2 tickets so Bob (0 tickets) should win
        var carol = _context.TeamMembers.First(m => m.Name == CAROL);
        _context.Tickets.AddRange(
            new Ticket { Title = "T1", AssignedTo = carol },
            new Ticket { Title = "T2", AssignedTo = carol },
            new Ticket { Title = "T3", AssignedTo = carol }
        );
        var bob = _context.TeamMembers.First(m => m.Name == BOB);
        _context.Tickets.AddRange(
            new Ticket { Title = "T1", AssignedTo = bob },
            new Ticket { Title = "T2", AssignedTo = bob },
            new Ticket { Title = "T3", AssignedTo = bob },
            new Ticket { Title = "T4", AssignedTo = bob }
        );
        var alice = _context.TeamMembers.First(m => m.Name == ALICE);
        _context.Tickets.AddRange(
            new Ticket { Title = "T1", AssignedTo = alice },
            new Ticket { Title = "T2", AssignedTo = alice },
            new Ticket { Title = "T3", AssignedTo = alice },
            new Ticket { Title = "T4", AssignedTo = alice }
        );
        await _context.SaveChangesAsync();

        var ticketService = new TicketService(_context);

        // Act
        var teamMember = await ticketService.FindFirstAvailableAsigneeNotAtTicketCapacityAsync();

        // Assert
        Assert.Equal(ALICE, teamMember?.Name);
    }
    
        
    [Fact]
    public async Task TicketService_FindAvailableAssigneeAsync_ShouldNotAssignToSomeoneUnderCapacityWithMultipleTickets()
    {
        // Arrange - give Alice 2 tickets so Bob (0 tickets) should win
        var carol = _context.TeamMembers.First(m => m.Name == CAROL);
        _context.Tickets.AddRange(
            new Ticket { Title = "T1", AssignedTo = carol },
            new Ticket { Title = "T2", AssignedTo = carol }
        );
        var bob = _context.TeamMembers.First(m => m.Name == BOB);
        _context.Tickets.AddRange(
            new Ticket { Title = "T1", AssignedTo = bob },
            new Ticket { Title = "T2", AssignedTo = bob },
            new Ticket { Title = "T3", AssignedTo = bob }
        );
        await _context.SaveChangesAsync();

        var ticketService = new TicketService(_context);

        // Act
        var teamMember = await ticketService.FindFirstAvailableAsigneeNotAtTicketCapacityAsync();

        // Assert
        Assert.Equal(ALICE, teamMember?.Name);
    }
}
