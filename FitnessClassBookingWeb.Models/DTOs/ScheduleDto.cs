using System.ComponentModel.DataAnnotations;

namespace FitnessClassBookingWeb.Models.DTOs;

public class ScheduleDto
{
    public int ScheduleId { get; set; }

    [Required]
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    [Required]
    public int RoomId { get; set; }

    public string? RoomName { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    public int? CurrentBookings { get; set; }

    public int? MaxParticipants { get; set; }
}
