using FitnessClassBookingWebApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClassBookingWebApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        /// <summary>
        /// Get all data for the home page
        /// </summary>
        /// <returns>Home page data including stats, featured groups, upcoming schedules, and recent reviews</returns>
        [HttpGet]
        public async Task<IActionResult> GetHomePageData()
        {
            try
            {
                var homePageData = await _homeService.GetHomePageDataAsync();
                return Ok(homePageData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving home page data", error = ex.Message });
            }
        }

        /// <summary>
        /// Get dashboard statistics
        /// </summary>
        /// <returns>Dashboard statistics</returns>
        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var stats = await _homeService.GetDashboardStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving statistics", error = ex.Message });
            }
        }

        /// <summary>
        /// Get featured groups (highest rated)
        /// </summary>
        /// <param name="count">Number of groups to return (default: 3)</param>
        /// <returns>List of featured groups</returns>
        [HttpGet("featured-groups")]
        public async Task<IActionResult> GetFeaturedGroups([FromQuery] int count = 3)
        {
            try
            {
                if (count < 1 || count > 10)
                {
                    return BadRequest(new { message = "Count must be between 1 and 10" });
                }

                var featuredGroups = await _homeService.GetFeaturedGroupsAsync(count);
                return Ok(featuredGroups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving featured groups", error = ex.Message });
            }
        }

        /// <summary>
        /// Get upcoming schedules
        /// </summary>
        /// <param name="count">Number of schedules to return (default: 5)</param>
        /// <returns>List of upcoming schedules</returns>
        [HttpGet("upcoming-schedules")]
        public async Task<IActionResult> GetUpcomingSchedules([FromQuery] int count = 5)
        {
            try
            {
                if (count < 1 || count > 20)
                {
                    return BadRequest(new { message = "Count must be between 1 and 20" });
                }

                var upcomingSchedules = await _homeService.GetUpcomingSchedulesAsync(count);
                return Ok(upcomingSchedules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving upcoming schedules", error = ex.Message });
            }
        }

        /// <summary>
        /// Get recent reviews
        /// </summary>
        /// <param name="count">Number of reviews to return (default: 5)</param>
        /// <returns>List of recent reviews</returns>
        [HttpGet("recent-reviews")]
        public async Task<IActionResult> GetRecentReviews([FromQuery] int count = 5)
        {
            try
            {
                if (count < 1 || count > 10)
                {
                    return BadRequest(new { message = "Count must be between 1 and 10" });
                }

                var recentReviews = await _homeService.GetRecentReviewsAsync(count);
                return Ok(recentReviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving recent reviews", error = ex.Message });
            }
        }
    }
}
