using FitnessClassBookingWeb.DataAccess.Repositories;
using FitnessClassBookingWeb.Models;

namespace FitnessClassBookingWeb.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<UserRole> UserRoles { get; }
        IRepository<Group> Groups { get; }
        IRepository<Schedule> Schedules { get; }
        IRepository<Room> Rooms { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<Review> Reviews { get; }
        IRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
