using FitnessClassBookingWeb.Models.DTOs;
using FitnessClassBookingWebApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClassBookingWebApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGroups()
    {
        var groups = await _groupService.GetAllGroupsAsync();
        return Ok(groups);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroupById(int id)
    {
        var group = await _groupService.GetGroupByIdAsync(id);
        if (group == null)
            return NotFound(new { message = "Group not found" });

        return Ok(group);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] GroupDto groupDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdGroup = await _groupService.CreateGroupAsync(groupDto);
        return CreatedAtAction(nameof(GetGroupById), new { id = createdGroup.GroupId }, createdGroup);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(int id, [FromBody] GroupDto groupDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedGroup = await _groupService.UpdateGroupAsync(id, groupDto);
        if (updatedGroup == null)
            return NotFound(new { message = "Group not found" });

        return Ok(updatedGroup);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var result = await _groupService.DeleteGroupAsync(id);
        if (!result)
            return NotFound(new { message = "Group not found" });

        return NoContent();
    }

    [HttpGet("coaches")]
    public async Task<IActionResult> GetAvailableCoaches()
    {
        var coaches = await _groupService.GetAvailableCoachesAsync();
        return Ok(coaches);
    }
}
