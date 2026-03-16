namespace FitnessClassBookingWeb.Models.DTOs
{
    public class HomePageDto
    {
        public DashboardStats Stats { get; set; } = new();
        public List<GroupDto> FeaturedGroups { get; set; } = new();
        public List<ScheduleDto> UpcomingSchedules { get; set; } = new();
        public List<ReviewDto> RecentReviews { get; set; } = new();
    }

    public class DashboardStats
    {
        public int TotalGroups { get; set; }
        public int TotalSchedules { get; set; }
        public int UpcomingSchedules { get; set; }
        public int TotalActiveUsers { get; set; }
        public int TotalBookings { get; set; }
        public double AverageRating { get; set; }
    }
}
