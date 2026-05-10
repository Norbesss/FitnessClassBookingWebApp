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
    public async Task<bool> CanUserReviewGroupAsync(int userId, int groupId)
    {
        return await _context.Bookings
            .Include(b => b.Schedule)
            .AnyAsync(b =>
                b.UserId == userId &&
                b.Schedule.GroupId == groupId &&
                b.Status != "Cancelled"
            );
    }

    public async Task<bool> HasUserAlreadyReviewedGroupAsync(int userId, int groupId)
    {
        return await _context.Reviews
            .AnyAsync(r => r.UserId == userId && r.GroupId == groupId);
    }

    public async Task<ReviewDto?> CreateReviewAsync(int userId, int groupId, CreateReviewRequest request)
    {
        var canReview = await CanUserReviewGroupAsync(userId, groupId);

        if (!canReview)
        {
            return null;
        }

        var alreadyReviewed = await HasUserAlreadyReviewedGroupAsync(userId, groupId);

        if (alreadyReviewed)
        {
            throw new InvalidOperationException("User has already reviewed this group.");
        }

        var review = new Review
        {
            UserId = userId,
            GroupId = groupId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var createdReview = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Group)
            .FirstAsync(r => r.ReviewId == review.ReviewId);

        return new ReviewDto
        {
            ReviewId = createdReview.ReviewId,
            UserId = createdReview.UserId,
            UserName = $"{createdReview.User.FirstName} {createdReview.User.LastName}",
            GroupId = createdReview.GroupId,
            GroupName = createdReview.Group.Name,
            Rating = createdReview.Rating,
            Comment = createdReview.Comment,
            CreatedAt = createdReview.CreatedAt
        };
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

    public async Task<bool> DeleteReviewAsync(int id, int userId, bool isAdmin)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return false;
        }

        if (!isAdmin && review.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ReviewDto?> GetUserReviewForGroupAsync(int userId, int groupId)
    {
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Group)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.GroupId == groupId);

        if (review == null)
        {
            return null;
        }

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

    public async Task<ReviewDto?> UpdateUserReviewForGroupAsync(int userId, int groupId, CreateReviewRequest request)
    {
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Group)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.GroupId == groupId);

        if (review == null)
        {
            return null;
        }

        review.Rating = request.Rating;
        review.Comment = request.Comment;

        await _context.SaveChangesAsync();

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
}
