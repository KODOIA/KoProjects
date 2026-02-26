using Microsoft.EntityFrameworkCore;
using Domain;
using Infrastructure.Data.Configurations;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
    }
}