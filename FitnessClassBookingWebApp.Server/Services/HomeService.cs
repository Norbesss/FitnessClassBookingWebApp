using FitnessClassBookingWeb.DataAccess.UnitOfWork;
using FitnessClassBookingWeb.Models.DTOs;

namespace FitnessClassBookingWebApp.Server.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HomePageDto> GetHomePageDataAsync()
        {
            var homePageData = new HomePageDto
            {
                Stats = await GetDashboardStatsAsync(),
                FeaturedGroups = (await GetFeaturedGroupsAsync(3)).ToList(),
                UpcomingSchedules = (await GetUpcomingSchedulesAsync(5)).ToList(),
                RecentReviews = (await GetRecentReviewsAsync(5)).ToList()
            };

            return homePageData;
        }

        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            var totalGroups = await _unitOfWork.Groups.CountAsync();
            var totalSchedules = await _unitOfWork.Schedules.CountAsync();
            var upcomingSchedules = await _unitOfWork.Schedules.CountAsync(
                s => s.StartTime > DateTime.UtcNow
            );
            var totalActiveUsers = await _unitOfWork.Users.CountAsync(u => u.IsActive);
            var totalBookings = await _unitOfWork.Bookings.CountAsync();

            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            var averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0.0;

            return new DashboardStats
            {
                TotalGroups = totalGroups,
                TotalSchedules = totalSchedules,
                UpcomingSchedules = upcomingSchedules,
                TotalActiveUsers = totalActiveUsers,
                TotalBookings = totalBookings,
                AverageRating = Math.Round(averageRating, 2)
            };
        }

        public async Task<IEnumerable<GroupDto>> GetFeaturedGroupsAsync(int count = 3)
        {
            // Get groups with their coaches
            var groups = await _unitOfWork.Groups.GetAllAsync(g => g.Coach);
            
            // Get reviews for each group to calculate average rating
            var allReviews = await _unitOfWork.Reviews.GetAllAsync();
            
            var groupDtos = groups.Select(g =>
            {
                var groupReviews = allReviews.Where(r => r.GroupId == g.GroupId).ToList();
                var averageRating = groupReviews.Any() ? groupReviews.Average(r => r.Rating) : 0.0;
                
                return new GroupDto
                {
                    GroupId = g.GroupId,
                    Name = g.Name,
                    Description = g.Description,
                    CoachId = g.CoachId,
                    CoachName = $"{g.Coach.FirstName} {g.Coach.LastName}",
                    MaxParticipants = g.MaxParticipants,
                    AverageRating = Math.Round(averageRating, 2),
                    TotalReviews = groupReviews.Count
                };
            })
            .OrderByDescending(g => g.AverageRating)
            .ThenByDescending(g => g.TotalReviews)
            .Take(count)
            .ToList();

            return groupDtos;
        }

        public async Task<IEnumerable<ScheduleDto>> GetUpcomingSchedulesAsync(int count = 5)
        {
            // Get schedules with related entities
            var schedules = await _unitOfWork.Schedules.FindAsync(
                s => s.StartTime > DateTime.UtcNow,
                s => s.Group,
                s => s.Room
            );

            // Get bookings count for each schedule
            var allBookings = await _unitOfWork.Bookings.GetAllAsync();

            var scheduleDtos = schedules
                .OrderBy(s => s.StartTime)
                .Take(count)
                .Select(s =>
                {
                    var currentBookings = allBookings.Count(b => 
                        b.ScheduleId == s.ScheduleId && 
                        b.Status == "Confirmed"
                    );

                    return new ScheduleDto
                    {
                        ScheduleId = s.ScheduleId,
                        GroupId = s.GroupId,
                        GroupName = s.Group.Name,
                        RoomId = s.RoomId,
                        RoomName = s.Room.Name,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        MaxParticipants = s.Group.MaxParticipants,
                        CurrentBookings = currentBookings
                    };
                })
                .ToList();

            return scheduleDtos;
        }

        public async Task<IEnumerable<ReviewDto>> GetRecentReviewsAsync(int count = 5)
        {
            // Get all reviews with related entities
            var reviews = await _unitOfWork.Reviews.GetAllAsync(
                r => r.User,
                r => r.Group
            );

            var reviewDtos = reviews
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
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
                .ToList();

            return reviewDtos;
        }
    }
}
