using FitnessClassBookingWeb.Models.DTOs;

namespace FitnessClassBookingWebApp.Server.Services;

public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
    Task<ReviewDto?> GetReviewByIdAsync(int id);
    Task<IEnumerable<ReviewDto>> GetReviewsByGroupIdAsync(int groupId);
    Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId);
    Task<ReviewDto> CreateReviewAsync(ReviewDto reviewDto);
    Task<ReviewDto?> UpdateReviewAsync(int id, ReviewDto reviewDto);
    Task<bool> DeleteReviewAsync(int id);
}
