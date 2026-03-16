using System.ComponentModel.DataAnnotations;

namespace FitnessClassBookingWeb.Models.DTOs;

public class BookingDto
{
    public int BookingId { get; set; }

    [Required]
    public int UserId { get; set; }

    public string? UserName { get; set; }

    [Required]
    public int ScheduleId { get; set; }

    public string? GroupName { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [Required]
    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }
}
