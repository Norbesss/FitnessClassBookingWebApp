using FitnessClassBookingWeb.DataAccess.Data;
using FitnessClassBookingWeb.DataAccess.Repositories;
using FitnessClassBookingWeb.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace FitnessClassBookingWeb.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private readonly Dictionary<Type, object> _repositories;

        private IRepository<User>? _users;
        private IRepository<Role>? _roles;
        private IRepository<UserRole>? _userRoles;
        private IRepository<Group>? _groups;
        private IRepository<Schedule>? _schedules;
        private IRepository<Room>? _rooms;
        private IRepository<Booking>? _bookings;
        private IRepository<Review>? _reviews;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IRepository<Role> Roles => _roles ??= new Repository<Role>(_context);
        public IRepository<UserRole> UserRoles => _userRoles ??= new Repository<UserRole>(_context);
        public IRepository<Group> Groups => _groups ??= new Repository<Group>(_context);
        public IRepository<Schedule> Schedules => _schedules ??= new Repository<Schedule>(_context);
        public IRepository<Room> Rooms => _rooms ??= new Repository<Room>(_context);
        public IRepository<Booking> Bookings => _bookings ??= new Repository<Booking>(_context);
        public IRepository<Review> Reviews => _reviews ??= new Repository<Review>(_context);

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<T>(_context);
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
