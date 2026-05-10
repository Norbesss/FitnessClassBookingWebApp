using System.Security.Claims;
using FitnessClassBookingWeb.Models.DTOs;
using FitnessClassBookingWebApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetReviewById(int id)
    {
        var review = await _reviewService.GetReviewByIdAsync(id);

        if (review == null)
        {
            return NotFound(new { message = "Review not found" });
        }

        return Ok(review);
    }

    [HttpGet("group/{groupId:int}")]
    public async Task<IActionResult> GetReviewsByGroup(int groupId)
    {
        var reviews = await _reviewService.GetReviewsByGroupIdAsync(groupId);
        return Ok(reviews);
    }

    [Authorize]
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetReviewsByUser(int userId)
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");

        if (!isAdmin && currentUserId != userId)
        {
            return Forbid();
        }

        var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
        return Ok(reviews);
    }

    [Authorize]
    [HttpPost("group/{groupId:int}")]
    public async Task<IActionResult> CreateReviewForGroup(
        int groupId,
        [FromBody] CreateReviewRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();

        try
        {
            var createdReview = await _reviewService.CreateReviewAsync(userId, groupId, request);

            if (createdReview == null)
            {
                return Forbid();
            }

            return CreatedAtAction(
                nameof(GetReviewById),
                new { id = createdReview.ReviewId },
                createdReview
            );
        }
        catch (InvalidOperationException)
        {
            return Conflict(new { message = "You have already reviewed this class." });
        }
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var userId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");

        try
        {
            var result = await _reviewService.DeleteReviewAsync(id, userId, isAdmin);

            if (!result)
            {
                return NotFound(new { message = "Review not found" });
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            User.FindFirst("sub")?.Value ??
            User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("User id claim is missing.");
        }

        return int.Parse(userIdClaim);
    }

    [Authorize]
    [HttpGet("group/{groupId:int}/me")]
    public async Task<IActionResult> GetMyReviewForGroup(int groupId)
    {
        var userId = GetCurrentUserId();

        var review = await _reviewService.GetUserReviewForGroupAsync(userId, groupId);

        if (review == null)
        {
            return NotFound(new { message = "Review not found." });
        }

        return Ok(review);
    }

    [Authorize]
    [HttpPut("group/{groupId:int}/me")]
    public async Task<IActionResult> UpdateMyReviewForGroup(
    int groupId,
    [FromBody] CreateReviewRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();

        var updatedReview = await _reviewService.UpdateUserReviewForGroupAsync(
            userId,
            groupId,
            request
        );

        if (updatedReview == null)
        {
            return NotFound(new { message = "You have not reviewed this class yet." });
        }

        return Ok(updatedReview);
    }
}
