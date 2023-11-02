using IASquad.Poc.AzureOpenAi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IASquad.Poc.AzureOpenAi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
    {
    }

    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>().ToTable("Messages");
    }

}
