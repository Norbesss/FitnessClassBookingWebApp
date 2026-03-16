using System.ComponentModel.DataAnnotations;

namespace FitnessClassBookingWeb.Models.DTOs;

public class ReviewDto
{
    public int ReviewId { get; set; }

    [Required]
    public int UserId { get; set; }

    public string? UserName { get; set; }

    [Required]
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; }
}
