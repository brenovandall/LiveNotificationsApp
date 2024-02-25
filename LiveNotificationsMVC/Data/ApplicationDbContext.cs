using LiveNotificationsMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LiveNotificationsMVC.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<HubNotification> HubNotifications { get; set; }
    public DbSet<Notifications> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var user = new User
        {
            Id = 1,
            Username = "Breno",
            Password = "Breno@1"
        };

        builder.Entity<User>().HasData(user);
    }
}
