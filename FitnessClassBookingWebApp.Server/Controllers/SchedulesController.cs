using FitnessClassBookingWeb.Models.DTOs;
using FitnessClassBookingWebApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClassBookingWebApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public SchedulesController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSchedules()
    {
        var schedules = await _scheduleService.GetAllSchedulesAsync();
        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetScheduleById(int id)
    {
        var schedule = await _scheduleService.GetScheduleByIdAsync(id);
        if (schedule == null)
            return NotFound(new { message = "Schedule not found" });

        return Ok(schedule);
    }

    [HttpGet("group/{groupId}")]
    public async Task<IActionResult> GetSchedulesByGroup(int groupId)
    {
        var schedules = await _scheduleService.GetSchedulesByGroupIdAsync(groupId);
        return Ok(schedules);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingSchedules()
    {
        var schedules = await _scheduleService.GetUpcomingSchedulesAsync();
        return Ok(schedules);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchedule([FromBody] ScheduleDto scheduleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdSchedule = await _scheduleService.CreateScheduleAsync(scheduleDto);
        return CreatedAtAction(nameof(GetScheduleById), new { id = createdSchedule.ScheduleId }, createdSchedule);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(int id, [FromBody] ScheduleDto scheduleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedSchedule = await _scheduleService.UpdateScheduleAsync(id, scheduleDto);
        if (updatedSchedule == null)
            return NotFound(new { message = "Schedule not found" });

        return Ok(updatedSchedule);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var result = await _scheduleService.DeleteScheduleAsync(id);
        if (!result)
            return NotFound(new { message = "Schedule not found" });

        return NoContent();
    }
}
