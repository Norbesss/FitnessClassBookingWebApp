using FitnessClassBookingWeb.Models.DTOs;

namespace FitnessClassBookingWebApp.Server.Services;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
    Task<BookingDto?> GetBookingByIdAsync(int id);
    Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(int userId);
    Task<IEnumerable<BookingDto>> GetBookingsByScheduleIdAsync(int scheduleId);
    Task<BookingDto?> CreateBookingAsync(int userId, int scheduleId);
    Task<bool> CancelBookingAsync(int id, int userId);
    Task<bool> UpdateBookingStatusAsync(int id, string status);
}
