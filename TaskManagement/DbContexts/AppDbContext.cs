using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace TaskManagement.DbContexts;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
}
