using FitnessClassBookingWeb.Models.DTOs;

namespace FitnessClassBookingWebApp.Server.Services
{
    public interface IHomeService
    {
        Task<HomePageDto> GetHomePageDataAsync();
        Task<DashboardStats> GetDashboardStatsAsync();
        Task<IEnumerable<GroupDto>> GetFeaturedGroupsAsync(int count = 3);
        Task<IEnumerable<ScheduleDto>> GetUpcomingSchedulesAsync(int count = 5);
        Task<IEnumerable<ReviewDto>> GetRecentReviewsAsync(int count = 5);
    }
}
