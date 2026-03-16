using FitnessClassBookingWeb.DataAccess.Data;
using FitnessClassBookingWeb.Models;
using FitnessClassBookingWeb.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FitnessClassBookingWebApp.Server.Services;

public class GroupService : IGroupService
{
    private readonly ApplicationDbContext _context;

    public GroupService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync()
    {
        var groups = await _context.Groups
            .Include(g => g.Coach)
            .Include(g => g.Reviews)
            .ToListAsync();

        return groups.Select(g =>
        {
            var averageRating = g.Reviews.Any() ? g.Reviews.Average(r => r.Rating) : 0.0;

            return new GroupDto
            {
                GroupId = g.GroupId,
                Name = g.Name,
                Description = g.Description,
                CoachId = g.CoachId,
                CoachName = $"{g.Coach.FirstName} {g.Coach.LastName}",
                MaxParticipants = g.MaxParticipants,
                AverageRating = Math.Round(averageRating, 2),
                TotalReviews = g.Reviews.Count
            };
        });
    }

    public async Task<GroupDto?> GetGroupByIdAsync(int id)
    {
        var group = await _context.Groups
            .Include(g => g.Coach)
            .Include(g => g.Reviews)
            .FirstOrDefaultAsync(g => g.GroupId == id);

        if (group == null)
            return null;

        var averageRating = group.Reviews.Any() ? group.Reviews.Average(r => r.Rating) : 0.0;

        return new GroupDto
        {
            GroupId = group.GroupId,
            Name = group.Name,
            Description = group.Description,
            CoachId = group.CoachId,
            CoachName = $"{group.Coach.FirstName} {group.Coach.LastName}",
            MaxParticipants = group.MaxParticipants,
            AverageRating = Math.Round(averageRating, 2),
            TotalReviews = group.Reviews.Count
        };
    }

    public async Task<GroupDto> CreateGroupAsync(GroupDto groupDto)
    {
        var group = new Group
        {
            Name = groupDto.Name,
            Description = groupDto.Description,
            CoachId = groupDto.CoachId,
            MaxParticipants = groupDto.MaxParticipants
        };

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        var coach = await _context.Users.FindAsync(groupDto.CoachId);
        groupDto.GroupId = group.GroupId;
        groupDto.CoachName = coach != null ? $"{coach.FirstName} {coach.LastName}" : null;
        groupDto.AverageRating = 0;
        groupDto.TotalReviews = 0;

        return groupDto;
    }

    public async Task<GroupDto?> UpdateGroupAsync(int id, GroupDto groupDto)
    {
        var group = await _context.Groups
            .Include(g => g.Reviews)
            .FirstOrDefaultAsync(g => g.GroupId == id);

        if (group == null)
            return null;

        group.Name = groupDto.Name;
        group.Description = groupDto.Description;
        group.CoachId = groupDto.CoachId;
        group.MaxParticipants = groupDto.MaxParticipants;

        await _context.SaveChangesAsync();

        var coach = await _context.Users.FindAsync(groupDto.CoachId);
        var averageRating = group.Reviews.Any() ? group.Reviews.Average(r => r.Rating) : 0.0;

        groupDto.GroupId = group.GroupId;
        groupDto.CoachName = coach != null ? $"{coach.FirstName} {coach.LastName}" : null;
        groupDto.AverageRating = Math.Round(averageRating, 2);
        groupDto.TotalReviews = group.Reviews.Count;

        return groupDto;
    }

    public async Task<bool> DeleteGroupAsync(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
            return false;

        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<User>> GetAvailableCoachesAsync()
    {
        var coachRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Coach");
        if (coachRole == null)
            return Enumerable.Empty<User>();

        var coachIds = await _context.UserRoles
            .Where(ur => ur.RoleId == coachRole.RoleId)
            .Select(ur => ur.UserId)
            .ToListAsync();

        return await _context.Users
            .Where(u => coachIds.Contains(u.UserId) && u.IsActive)
            .ToListAsync();
    }
}
