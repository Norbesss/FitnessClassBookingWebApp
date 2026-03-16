using FitnessClassBookingWeb.Models.DTOs;
using FitnessClassBookingWebApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClassBookingWebApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReviews()
    {
        var reviews = await _reviewService.GetAllReviewsAsync();
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewById(int id)
    {
        var review = await _reviewService.GetReviewByIdAsync(id);
        if (review == null)
            return NotFound(new { message = "Review not found" });

        return Ok(review);
    }

    [HttpGet("group/{groupId}")]
    public async Task<IActionResult> GetReviewsByGroup(int groupId)
    {
        var reviews = await _reviewService.GetReviewsByGroupIdAsync(groupId);
        return Ok(reviews);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetReviewsByUser(int userId)
    {
        var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdReview = await _reviewService.CreateReviewAsync(reviewDto);
        return CreatedAtAction(nameof(GetReviewById), new { id = createdReview.ReviewId }, createdReview);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDto reviewDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedReview = await _reviewService.UpdateReviewAsync(id, reviewDto);
        if (updatedReview == null)
            return NotFound(new { message = "Review not found" });

        return Ok(updatedReview);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var result = await _reviewService.DeleteReviewAsync(id);
        if (!result)
            return NotFound(new { message = "Review not found" });

        return NoContent();
    }
}
