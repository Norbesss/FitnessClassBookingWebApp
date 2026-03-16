using FitnessClassBookingWeb.DataAccess.Data;
using FitnessClassBookingWeb.Models;
using FitnessClassBookingWeb.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FitnessClassBookingWebApp.Server.Services;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
    {
        return await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Schedule)
            .ThenInclude(s => s.Group)
            .Select(b => new BookingDto
            {
                BookingId = b.BookingId,
                UserId = b.UserId,
                UserName = $"{b.User.FirstName} {b.User.LastName}",
                ScheduleId = b.ScheduleId,
                GroupName = b.Schedule.Group.Name,
                StartTime = b.Schedule.StartTime,
                EndTime = b.Schedule.EndTime,
                Status = b.Status,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Schedule)
            .ThenInclude(s => s.Group)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null)
            return null;

        return new BookingDto
        {
            BookingId = booking.BookingId,
            UserId = booking.UserId,
            UserName = $"{booking.User.FirstName} {booking.User.LastName}",
            ScheduleId = booking.ScheduleId,
            GroupName = booking.Schedule.Group.Name,
            StartTime = booking.Schedule.StartTime,
            EndTime = booking.Schedule.EndTime,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt
        };
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(int userId)
    {
        return await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Schedule)
            .ThenInclude(s => s.Group)
            .Where(b => b.UserId == userId)
            .Select(b => new BookingDto
            {
                BookingId = b.BookingId,
                UserId = b.UserId,
                UserName = $"{b.User.FirstName} {b.User.LastName}",
                ScheduleId = b.ScheduleId,
                GroupName = b.Schedule.Group.Name,
                StartTime = b.Schedule.StartTime,
                EndTime = b.Schedule.EndTime,
                Status = b.Status,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByScheduleIdAsync(int scheduleId)
    {
        return await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Schedule)
            .ThenInclude(s => s.Group)
            .Where(b => b.ScheduleId == scheduleId)
            .Select(b => new BookingDto
            {
                BookingId = b.BookingId,
                UserId = b.UserId,
                UserName = $"{b.User.FirstName} {b.User.LastName}",
                ScheduleId = b.ScheduleId,
                GroupName = b.Schedule.Group.Name,
                StartTime = b.Schedule.StartTime,
                EndTime = b.Schedule.EndTime,
                Status = b.Status,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<BookingDto?> CreateBookingAsync(int userId, int scheduleId)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Group)
            .Include(s => s.Bookings)
            .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

        if (schedule == null)
            return null;

        var confirmedBookings = schedule.Bookings.Count(b => b.Status == "Confirmed");
        if (confirmedBookings >= schedule.Group.MaxParticipants)
            return null;

        var existingBooking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.UserId == userId && b.ScheduleId == scheduleId);

        if (existingBooking != null)
            return null;

        var booking = new Booking
        {
            UserId = userId,
            ScheduleId = scheduleId,
            Status = "Confirmed",
            CreatedAt = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);

        return new BookingDto
        {
            BookingId = booking.BookingId,
            UserId = booking.UserId,
            UserName = user != null ? $"{user.FirstName} {user.LastName}" : null,
            ScheduleId = booking.ScheduleId,
            GroupName = schedule.Group.Name,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt
        };
    }

    public async Task<bool> CancelBookingAsync(int id, int userId)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null || booking.UserId != userId)
            return false;

        booking.Status = "Cancelled";
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateBookingStatusAsync(int id, string status)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return false;

        booking.Status = status;
        await _context.SaveChangesAsync();

        return true;
    }
}
