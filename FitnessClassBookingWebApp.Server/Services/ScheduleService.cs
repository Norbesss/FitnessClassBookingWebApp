using FitnessClassBookingWeb.DataAccess.Data;
using FitnessClassBookingWeb.Models;
using FitnessClassBookingWeb.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FitnessClassBookingWebApp.Server.Services;

public class ScheduleService : IScheduleService
{
    private readonly ApplicationDbContext _context;

    public ScheduleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync()
    {
        return await _context.Schedules
            .Include(s => s.Group)
            .Include(s => s.Room)
            .Include(s => s.Bookings)
            .Select(s => new ScheduleDto
            {
                ScheduleId = s.ScheduleId,
                GroupId = s.GroupId,
                GroupName = s.Group.Name,
                RoomId = s.RoomId,
                RoomName = s.Room.Name,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                CurrentBookings = s.Bookings.Count(b => b.Status == "Confirmed"),
                MaxParticipants = s.Group.MaxParticipants
            })
            .ToListAsync();
    }

    public async Task<ScheduleDto?> GetScheduleByIdAsync(int id)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Group)
            .Include(s => s.Room)
            .Include(s => s.Bookings)
            .FirstOrDefaultAsync(s => s.ScheduleId == id);

        if (schedule == null)
            return null;

        return new ScheduleDto
        {
            ScheduleId = schedule.ScheduleId,
            GroupId = schedule.GroupId,
            GroupName = schedule.Group.Name,
            RoomId = schedule.RoomId,
            RoomName = schedule.Room.Name,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            CurrentBookings = schedule.Bookings.Count(b => b.Status == "Confirmed"),
            MaxParticipants = schedule.Group.MaxParticipants
        };
    }

    public async Task<IEnumerable<ScheduleDto>> GetSchedulesByGroupIdAsync(int groupId)
    {
        return await _context.Schedules
            .Include(s => s.Group)
            .Include(s => s.Room)
            .Include(s => s.Bookings)
            .Where(s => s.GroupId == groupId)
            .Select(s => new ScheduleDto
            {
                ScheduleId = s.ScheduleId,
                GroupId = s.GroupId,
                GroupName = s.Group.Name,
                RoomId = s.RoomId,
                RoomName = s.Room.Name,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                CurrentBookings = s.Bookings.Count(b => b.Status == "Confirmed"),
                MaxParticipants = s.Group.MaxParticipants
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ScheduleDto>> GetUpcomingSchedulesAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Schedules
            .Include(s => s.Group)
            .Include(s => s.Room)
            .Include(s => s.Bookings)
            .Where(s => s.StartTime > now)
            .OrderBy(s => s.StartTime)
            .Select(s => new ScheduleDto
            {
                ScheduleId = s.ScheduleId,
                GroupId = s.GroupId,
                GroupName = s.Group.Name,
                RoomId = s.RoomId,
                RoomName = s.Room.Name,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                CurrentBookings = s.Bookings.Count(b => b.Status == "Confirmed"),
                MaxParticipants = s.Group.MaxParticipants
            })
            .ToListAsync();
    }

    public async Task<ScheduleDto> CreateScheduleAsync(ScheduleDto scheduleDto)
    {
        var schedule = new Schedule
        {
            GroupId = scheduleDto.GroupId,
            RoomId = scheduleDto.RoomId,
            StartTime = scheduleDto.StartTime,
            EndTime = scheduleDto.EndTime
        };

        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();

        var group = await _context.Groups.FindAsync(scheduleDto.GroupId);
        var room = await _context.Rooms.FindAsync(scheduleDto.RoomId);

        scheduleDto.ScheduleId = schedule.ScheduleId;
        scheduleDto.GroupName = group?.Name;
        scheduleDto.RoomName = room?.Name;
        scheduleDto.CurrentBookings = 0;
        scheduleDto.MaxParticipants = group?.MaxParticipants;

        return scheduleDto;
    }

    public async Task<ScheduleDto?> UpdateScheduleAsync(int id, ScheduleDto scheduleDto)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule == null)
            return null;

        schedule.GroupId = scheduleDto.GroupId;
        schedule.RoomId = scheduleDto.RoomId;
        schedule.StartTime = scheduleDto.StartTime;
        schedule.EndTime = scheduleDto.EndTime;

        await _context.SaveChangesAsync();

        var group = await _context.Groups.FindAsync(scheduleDto.GroupId);
        var room = await _context.Rooms.FindAsync(scheduleDto.RoomId);

        scheduleDto.ScheduleId = schedule.ScheduleId;
        scheduleDto.GroupName = group?.Name;
        scheduleDto.RoomName = room?.Name;

        return scheduleDto;
    }

    public async Task<bool> DeleteScheduleAsync(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule == null)
            return false;

        _context.Schedules.Remove(schedule);
        await _context.SaveChangesAsync();

        return true;
    }
}
