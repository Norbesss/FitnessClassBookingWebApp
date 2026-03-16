using System.ComponentModel.DataAnnotations;

namespace FitnessClassBookingWeb.Models.DTOs;

public class GroupDto
{
    public int GroupId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public int CoachId { get; set; }

    public string? CoachName { get; set; }

    [Required]
    [Range(1, 100)]
    public int MaxParticipants { get; set; }

    // Additional properties for display
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
}
