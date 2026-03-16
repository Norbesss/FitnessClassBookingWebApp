using FitnessClassBookingWeb.Models.DTOs;

namespace FitnessClassBookingWebApp.Server.Services;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync();
    Task<ScheduleDto?> GetScheduleByIdAsync(int id);
    Task<IEnumerable<ScheduleDto>> GetSchedulesByGroupIdAsync(int groupId);
    Task<IEnumerable<ScheduleDto>> GetUpcomingSchedulesAsync();
    Task<ScheduleDto> CreateScheduleAsync(ScheduleDto scheduleDto);
    Task<ScheduleDto?> UpdateScheduleAsync(int id, ScheduleDto scheduleDto);
    Task<bool> DeleteScheduleAsync(int id);
}
