using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace TaskManagement.DbContexts;


public class AppDbContext : IdentityDbContext<ApplicationAccount, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    #region
    public DbSet<TaskInfs> Tasks { get; set; }
    public DbSet<UserTasks> UserTasks { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserTasks>()
        .HasKey(ut => new { ut.UserId, ut.TaskId });

        modelBuilder.Entity<UserTasks>()
            .HasOne(ut => ut.User)
            .WithMany(u => u.UserTasks)
            .HasForeignKey(ut => ut.UserId);

        modelBuilder.Entity<UserTasks>()
            .HasOne(ut => ut.Task)
            .WithMany(t => t.UserTasks)
            .HasForeignKey(ut => ut.TaskId);

    }
} 

