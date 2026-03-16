using Microsoft.EntityFrameworkCore;
using FitnessClassBookingWeb.Models;

namespace FitnessClassBookingWeb.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<Schedule> Schedules { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Group>()
            .HasOne(g => g.Coach)
            .WithMany(u => u.CoachingGroups)
            .HasForeignKey(g => g.CoachId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Group)
            .WithMany(g => g.Schedules)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Room)
            .WithMany(r => r.Schedules)
            .HasForeignKey(s => s.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Schedule)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.ScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Group)
            .WithMany(g => g.Reviews)
            .HasForeignKey(r => r.GroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
