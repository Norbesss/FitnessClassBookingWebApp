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
