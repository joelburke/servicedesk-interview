using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Models;

namespace ServiceDesk.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamMember>().HasData(
            new TeamMember { Id = 1, Name = "Alice", MaxCapacity = 5 },
            new TeamMember { Id = 2, Name = "Bob", MaxCapacity = 5 },
            new TeamMember { Id = 3, Name = "Carol", MaxCapacity = 3 }
        );
    }
}
