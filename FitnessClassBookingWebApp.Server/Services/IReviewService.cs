using FitnessClassBookingWeb.Models.DTOs;

namespace FitnessClassBookingWebApp.Server.Services;

public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
    Task<ReviewDto?> GetReviewByIdAsync(int id);
    Task<IEnumerable<ReviewDto>> GetReviewsByGroupIdAsync(int groupId);
    Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId);
    Task<ReviewDto?> CreateReviewAsync(int userId, int groupId, CreateReviewRequest request);
    Task<ReviewDto?> UpdateReviewAsync(int id, ReviewDto reviewDto);
    Task<bool> CanUserReviewGroupAsync(int userId, int groupId);
    Task<bool> HasUserAlreadyReviewedGroupAsync(int userId, int groupId);
    Task<bool> DeleteReviewAsync(int id, int userId, bool isAdmin);
    Task<ReviewDto?> GetUserReviewForGroupAsync(int userId, int groupId);
    Task<ReviewDto?> UpdateUserReviewForGroupAsync(int userId, int groupId, CreateReviewRequest request);
}
