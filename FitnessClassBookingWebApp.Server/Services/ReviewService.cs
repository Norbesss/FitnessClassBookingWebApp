using FitnessClassBookingWeb.DataAccess.Data;
using FitnessClassBookingWeb.Models;
using FitnessClassBookingWeb.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FitnessClassBookingWebApp.Server.Services;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Group)
            .Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                UserName = $"{r.User.FirstName} {r.User.LastName}",
                GroupId = r.GroupId,
                GroupName = r.Group.Name,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ReviewDto?> GetReviewByIdAsync(int id)
    {
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Group)
            .FirstOrDefaultAsync(r => r.ReviewId == id);

        if (review == null)
            return null;

        return new ReviewDto
        {
            ReviewId = review.ReviewId,
            UserId = review.UserId,
            UserName = $"{review.User.FirstName} {review.User.LastName}",
            GroupId = review.GroupId,
            GroupName = review.Group.Name,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsByGroupIdAsync(int groupId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Group)
            .Where(r => r.GroupId == groupId)
            .Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                UserName = $"{r.User.FirstName} {r.User.LastName}",
                GroupId = r.GroupId,
                GroupName = r.Group.Name,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Group)
            .Where(r => r.UserId == userId)
            .Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                UserName = $"{r.User.FirstName} {r.User.LastName}",
                GroupId = r.GroupId,
                GroupName = r.Group.Name,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ReviewDto> CreateReviewAsync(ReviewDto reviewDto)
    {
        var review = new Review
        {
            UserId = reviewDto.UserId,
            GroupId = reviewDto.GroupId,
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(reviewDto.UserId);
        var group = await _context.Groups.FindAsync(reviewDto.GroupId);

        reviewDto.ReviewId = review.ReviewId;
        reviewDto.UserName = user != null ? $"{user.FirstName} {user.LastName}" : null;
        reviewDto.GroupName = group?.Name;
        reviewDto.CreatedAt = review.CreatedAt;

        return reviewDto;
    }

    public async Task<ReviewDto?> UpdateReviewAsync(int id, ReviewDto reviewDto)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
            return null;

        review.Rating = reviewDto.Rating;
        review.Comment = reviewDto.Comment;

        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(review.UserId);
        var group = await _context.Groups.FindAsync(review.GroupId);

        reviewDto.ReviewId = review.ReviewId;
        reviewDto.UserId = review.UserId;
        reviewDto.GroupId = review.GroupId;
        reviewDto.UserName = user != null ? $"{user.FirstName} {user.LastName}" : null;
        reviewDto.GroupName = group?.Name;
        reviewDto.CreatedAt = review.CreatedAt;

        return reviewDto;
    }

    public async Task<bool> DeleteReviewAsync(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
            return false;

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return true;
    }
}
