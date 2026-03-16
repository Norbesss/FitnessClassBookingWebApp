using FitnessClassBookingWeb.DataAccess.Data;
using FitnessClassBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessClassBookingWebApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Select(u => new
            {
                u.UserId,
                u.FirstName,
                u.LastName,
                u.Email,
                u.PhoneNumber,
                u.CreatedAt,
                u.IsActive,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.UserId == id)
            .Select(u => new
            {
                u.UserId,
                u.FirstName,
                u.LastName,
                u.Email,
                u.PhoneNumber,
                u.CreatedAt,
                u.IsActive,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }

    [HttpPatch("users/{id}/toggle-active")]
    public async Task<IActionResult> ToggleUserActive(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found" });

        user.IsActive = !user.IsActive;
        await _context.SaveChangesAsync();

        return Ok(new { userId = user.UserId, isActive = user.IsActive });
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _context.Roles.ToListAsync();
        return Ok(roles);
    }

    [HttpPost("users/{userId}/roles/{roleId}")]
    public async Task<IActionResult> AssignRoleToUser(int userId, int roleId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var role = await _context.Roles.FindAsync(roleId);
        if (role == null)
            return NotFound(new { message = "Role not found" });

        var existingUserRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (existingUserRole != null)
            return BadRequest(new { message = "User already has this role" });

        _context.UserRoles.Add(new UserRole
        {
            UserId = userId,
            RoleId = roleId
        });
        await _context.SaveChangesAsync();

        return Ok(new { message = "Role assigned successfully" });
    }

    [HttpDelete("users/{userId}/roles/{roleId}")]
    public async Task<IActionResult> RemoveRoleFromUser(int userId, int roleId)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (userRole == null)
            return NotFound(new { message = "User role not found" });

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("rooms")]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _context.Rooms.ToListAsync();
        return Ok(rooms);
    }

    [HttpGet("rooms/{id}")]
    public async Task<IActionResult> GetRoomById(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound(new { message = "Room not found" });

        return Ok(room);
    }

    [HttpPost("rooms")]
    public async Task<IActionResult> CreateRoom([FromBody] Room room)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoomById), new { id = room.RoomId }, room);
    }

    [HttpPut("rooms/{id}")]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room room)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingRoom = await _context.Rooms.FindAsync(id);
        if (existingRoom == null)
            return NotFound(new { message = "Room not found" });

        existingRoom.Name = room.Name;
        existingRoom.Capacity = room.Capacity;

        await _context.SaveChangesAsync();

        return Ok(existingRoom);
    }

    [HttpDelete("rooms/{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound(new { message = "Room not found" });

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var stats = new
        {
            TotalUsers = await _context.Users.CountAsync(),
            ActiveUsers = await _context.Users.CountAsync(u => u.IsActive),
            TotalGroups = await _context.Groups.CountAsync(),
            TotalSchedules = await _context.Schedules.CountAsync(),
            UpcomingSchedules = await _context.Schedules.CountAsync(s => s.StartTime > DateTime.UtcNow),
            TotalBookings = await _context.Bookings.CountAsync(),
            ConfirmedBookings = await _context.Bookings.CountAsync(b => b.Status == "Confirmed"),
            TotalReviews = await _context.Reviews.CountAsync(),
            AverageRating = await _context.Reviews.AnyAsync() ? await _context.Reviews.AverageAsync(r => r.Rating) : 0
        };

        return Ok(stats);
    }
}
