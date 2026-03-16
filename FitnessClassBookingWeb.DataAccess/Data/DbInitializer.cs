using FitnessClassBookingWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClassBookingWeb.DataAccess.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any())
        {
            return;
        }

        var roles = new Role[]
        {
            new Role { Name = "Admin" },
            new Role { Name = "Coach" },
            new Role { Name = "Member" }
        };

        context.Roles.AddRange(roles);
        context.SaveChanges();

        var users = new User[]
        {
            new User
            {
                FirstName = "John",
                LastName = "Admin",
                Email = "admin@fitness.com",
                PasswordHash = "hashed_password_123",
                PhoneNumber = "+1234567890",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FirstName = "Sarah",
                LastName = "Johnson",
                Email = "sarah.johnson@fitness.com",
                PasswordHash = "hashed_password_456",
                PhoneNumber = "+1234567891",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FirstName = "Mike",
                LastName = "Thompson",
                Email = "mike.thompson@fitness.com",
                PasswordHash = "hashed_password_789",
                PhoneNumber = "+1234567892",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FirstName = "Emily",
                LastName = "Davis",
                Email = "emily.davis@email.com",
                PasswordHash = "hashed_password_101",
                PhoneNumber = "+1234567893",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FirstName = "Chris",
                LastName = "Wilson",
                Email = "chris.wilson@email.com",
                PasswordHash = "hashed_password_102",
                PhoneNumber = "+1234567894",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FirstName = "Lisa",
                LastName = "Martinez",
                Email = "lisa.martinez@email.com",
                PasswordHash = "hashed_password_103",
                PhoneNumber = "+1234567895",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();

        var userRoles = new UserRole[]
        {
            new UserRole { UserId = users[0].UserId, RoleId = roles[0].RoleId },
            new UserRole { UserId = users[1].UserId, RoleId = roles[1].RoleId },
            new UserRole { UserId = users[2].UserId, RoleId = roles[1].RoleId },
            new UserRole { UserId = users[3].UserId, RoleId = roles[2].RoleId },
            new UserRole { UserId = users[4].UserId, RoleId = roles[2].RoleId },
            new UserRole { UserId = users[5].UserId, RoleId = roles[2].RoleId }
        };

        context.UserRoles.AddRange(userRoles);
        context.SaveChanges();

        var rooms = new Room[]
        {
            new Room { Name = "Studio A", Capacity = 30 },
            new Room { Name = "Studio B", Capacity = 25 },
            new Room { Name = "Yoga Room", Capacity = 20 },
            new Room { Name = "Spin Room", Capacity = 15 },
            new Room { Name = "Outdoor Area", Capacity = 40 }
        };

        context.Rooms.AddRange(rooms);
        context.SaveChanges();

        var groups = new Group[]
        {
            new Group
            {
                Name = "Morning Yoga",
                Description = "Start your day with energizing yoga session suitable for all levels",
                CoachId = users[1].UserId,
                MaxParticipants = 20
            },
            new Group
            {
                Name = "HIIT Cardio",
                Description = "High-intensity interval training to boost your metabolism",
                CoachId = users[1].UserId,
                MaxParticipants = 25
            },
            new Group
            {
                Name = "Spin Class",
                Description = "Indoor cycling class with energetic music and motivation",
                CoachId = users[2].UserId,
                MaxParticipants = 15
            },
            new Group
            {
                Name = "Pilates Core",
                Description = "Strengthen your core with controlled movements",
                CoachId = users[2].UserId,
                MaxParticipants = 20
            },
            new Group
            {
                Name = "Zumba Dance",
                Description = "Dance fitness party with Latin-inspired music",
                CoachId = users[1].UserId,
                MaxParticipants = 30
            }
        };

        context.Groups.AddRange(groups);
        context.SaveChanges();

        var now = DateTime.UtcNow;
        var schedules = new Schedule[]
        {
            new Schedule
            {
                GroupId = groups[0].GroupId,
                RoomId = rooms[2].RoomId,
                StartTime = now.AddDays(1).Date.AddHours(7),
                EndTime = now.AddDays(1).Date.AddHours(8)
            },
            new Schedule
            {
                GroupId = groups[0].GroupId,
                RoomId = rooms[2].RoomId,
                StartTime = now.AddDays(3).Date.AddHours(7),
                EndTime = now.AddDays(3).Date.AddHours(8)
            },
            new Schedule
            {
                GroupId = groups[1].GroupId,
                RoomId = rooms[0].RoomId,
                StartTime = now.AddDays(1).Date.AddHours(18),
                EndTime = now.AddDays(1).Date.AddHours(19)
            },
            new Schedule
            {
                GroupId = groups[1].GroupId,
                RoomId = rooms[0].RoomId,
                StartTime = now.AddDays(2).Date.AddHours(18),
                EndTime = now.AddDays(2).Date.AddHours(19)
            },
            new Schedule
            {
                GroupId = groups[2].GroupId,
                RoomId = rooms[3].RoomId,
                StartTime = now.AddDays(2).Date.AddHours(17),
                EndTime = now.AddDays(2).Date.AddHours(18)
            },
            new Schedule
            {
                GroupId = groups[3].GroupId,
                RoomId = rooms[2].RoomId,
                StartTime = now.AddDays(1).Date.AddHours(12),
                EndTime = now.AddDays(1).Date.AddHours(13)
            },
            new Schedule
            {
                GroupId = groups[4].GroupId,
                RoomId = rooms[0].RoomId,
                StartTime = now.AddDays(3).Date.AddHours(19),
                EndTime = now.AddDays(3).Date.AddHours(20)
            }
        };

        context.Schedules.AddRange(schedules);
        context.SaveChanges();

        var bookings = new Booking[]
        {
            new Booking
            {
                UserId = users[3].UserId,
                ScheduleId = schedules[0].ScheduleId,
                Status = "Confirmed",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Booking
            {
                UserId = users[4].UserId,
                ScheduleId = schedules[0].ScheduleId,
                Status = "Confirmed",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Booking
            {
                UserId = users[5].UserId,
                ScheduleId = schedules[2].ScheduleId,
                Status = "Confirmed",
                CreatedAt = DateTime.UtcNow.AddHours(-12)
            },
            new Booking
            {
                UserId = users[3].UserId,
                ScheduleId = schedules[4].ScheduleId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow.AddHours(-3)
            },
            new Booking
            {
                UserId = users[4].UserId,
                ScheduleId = schedules[6].ScheduleId,
                Status = "Confirmed",
                CreatedAt = DateTime.UtcNow.AddHours(-6)
            }
        };

        context.Bookings.AddRange(bookings);
        context.SaveChanges();

        var reviews = new Review[]
        {
            new Review
            {
                UserId = users[3].UserId,
                GroupId = groups[0].GroupId,
                Rating = 5,
                Comment = "Amazing class! Sarah is a fantastic instructor. I feel so energized after every session.",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Review
            {
                UserId = users[4].UserId,
                GroupId = groups[1].GroupId,
                Rating = 4,
                Comment = "Great workout! Very challenging but worth it. Could use more variety in exercises.",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Review
            {
                UserId = users[5].UserId,
                GroupId = groups[2].GroupId,
                Rating = 5,
                Comment = "Best spin class ever! Mike knows how to motivate and push you to your limits.",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Review
            {
                UserId = users[3].UserId,
                GroupId = groups[4].GroupId,
                Rating = 5,
                Comment = "So much fun! Doesn't even feel like exercising. Highly recommend!",
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            }
        };

        context.Reviews.AddRange(reviews);
        context.SaveChanges();
    }
}
