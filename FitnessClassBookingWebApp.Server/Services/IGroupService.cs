using FitnessClassBookingWeb.Models;
using FitnessClassBookingWeb.Models.DTOs;

namespace FitnessClassBookingWebApp.Server.Services;

public interface IGroupService
{
    Task<IEnumerable<GroupDto>> GetAllGroupsAsync();
    Task<GroupDto?> GetGroupByIdAsync(int id);
    Task<GroupDto> CreateGroupAsync(GroupDto groupDto);
    Task<GroupDto?> UpdateGroupAsync(int id, GroupDto groupDto);
    Task<bool> DeleteGroupAsync(int id);
    Task<IEnumerable<User>> GetAvailableCoachesAsync();
}
