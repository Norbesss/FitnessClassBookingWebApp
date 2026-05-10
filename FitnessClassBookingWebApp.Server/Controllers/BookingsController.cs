using FitnessClassBookingWebApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClassBookingWebApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        return Ok(bookings);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null)
        {
            return NotFound(new { message = "Booking not found" });
        }

        return Ok(booking);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetBookingsByUserId(int userId)
    {
        var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
        return Ok(bookings);
    }

    [HttpGet("schedule/{scheduleId:int}")]
    public async Task<IActionResult> GetBookingsByScheduleId(int scheduleId)
    {
        var bookings = await _bookingService.GetBookingsByScheduleIdAsync(scheduleId);
        return Ok(bookings);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var booking = await _bookingService.CreateBookingAsync(request.UserId, request.ScheduleId);
        if (booking == null)
        {
            return BadRequest(new { message = "Unable to create booking" });
        }

        return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingId }, booking);
    }

    [Authorize]
    [HttpPatch("{id:int}/cancel")]
    public async Task<IActionResult> CancelBooking(int id, [FromBody] CancelBookingRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _bookingService.CancelBookingAsync(id, request.UserId);
        if (!result)
        {
            return NotFound(new { message = "Booking not found" });
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin,Coach")]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] UpdateBookingStatusRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _bookingService.UpdateBookingStatusAsync(id, request.Status);
        if (!result)
        {
            return NotFound(new { message = "Booking not found" });
        }

        return NoContent();
    }

    public sealed record CreateBookingRequest(int UserId, int ScheduleId);
    public sealed record CancelBookingRequest(int UserId);
    public sealed record UpdateBookingStatusRequest(string Status);
}
