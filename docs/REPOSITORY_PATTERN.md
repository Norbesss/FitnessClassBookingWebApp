# 🏗️ Repository & Unit of Work Pattern Implementation

## ✅ What Has Been Created

I've implemented the **Repository Pattern** and **Unit of Work Pattern** for your ASP.NET Entity Framework project. This provides better abstraction, testability, and transaction management.

---

## 📦 Files Created (4 new files)

### 1. Generic Repository Interface
**File:** `FitnessClassBookingWeb.DataAccess/Repositories/IRepository.cs`

Provides generic CRUD operations for all entities:
- Query methods (GetById, GetAll, Find, etc.)
- Command methods (Add, Update, Remove, etc.)
- Support for eager loading with Include
- Async/await support

### 2. Generic Repository Implementation
**File:** `FitnessClassBookingWeb.DataAccess/Repositories/Repository.cs`

Implements the generic repository with:
- DbSet abstraction
- LINQ expression support
- Include support for related entities
- Async operations

### 3. Unit of Work Interface
**File:** `FitnessClassBookingWeb.DataAccess/UnitOfWork/IUnitOfWork.cs`

Defines:
- Repository properties for each entity
- Generic repository access
- Transaction management methods
- IDisposable implementation

### 4. Unit of Work Implementation
**File:** `FitnessClassBookingWeb.DataAccess/UnitOfWork/UnitOfWork.cs`

Implements:
- Lazy-loaded repositories
- Transaction handling
- SaveChanges coordination
- Proper disposal

---

## 🔧 Configuration Updates

### Program.cs
Added Unit of Work registration:
```csharp
// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```

### AuthService.cs
Updated to use Unit of Work instead of direct DbContext access.

---

## 💡 How to Use

### Basic CRUD Operations

```csharp
public class ExampleService
{
    private readonly IUnitOfWork _unitOfWork;

    public ExampleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET - Retrieve all users
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _unitOfWork.Users.GetAllAsync();
    }

    // GET - Retrieve user with related data (eager loading)
    public async Task<User?> GetUserWithRolesAsync(int userId)
    {
        return await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.UserId == userId,
            u => u.UserRoles  // Include navigation property
        );
    }

    // GET - Find users matching criteria
    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _unitOfWork.Users.FindAsync(u => u.IsActive);
    }

    // CREATE - Add new user
    public async Task<User> CreateUserAsync(User user)
    {
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return user;
    }

    // UPDATE - Update existing user
    public async Task<bool> UpdateUserAsync(int userId, User updatedUser)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return false;

        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        // Update other properties...

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    // DELETE - Remove user
    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return false;

        _unitOfWork.Users.Remove(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
```

### Using Transactions

```csharp
public async Task<bool> CreateBookingWithTransaction(Booking booking)
{
    try
    {
        await _unitOfWork.BeginTransactionAsync();

        // Add booking
        await _unitOfWork.Bookings.AddAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        // Update schedule capacity
        var schedule = await _unitOfWork.Schedules.GetByIdAsync(booking.ScheduleId);
        if (schedule != null)
        {
            // Business logic here...
            _unitOfWork.Schedules.Update(schedule);
            await _unitOfWork.SaveChangesAsync();
        }

        await _unitOfWork.CommitTransactionAsync();
        return true;
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();
        throw;
    }
}
```

### Complex Queries with Multiple Includes

```csharp
public async Task<Group?> GetGroupWithDetailsAsync(int groupId)
{
    return await _unitOfWork.Groups.FirstOrDefaultAsync(
        g => g.GroupId == groupId,
        g => g.Coach,        // Include coach
        g => g.Schedules,    // Include schedules
        g => g.Reviews       // Include reviews
    );
}
```

### Using Generic Repository

```csharp
// If you need a repository for an entity not explicitly defined
public async Task<IEnumerable<T>> GetAllEntitiesAsync<T>() where T : class
{
    return await _unitOfWork.Repository<T>().GetAllAsync();
}
```

---

## 🎯 Benefits

### 1. **Separation of Concerns**
- Data access logic abstracted from business logic
- Services don't directly depend on EF Core

### 2. **Testability**
- Easy to mock `IUnitOfWork` and `IRepository<T>`
- No need to mock `DbContext` or `DbSet`

```csharp
// Unit test example
[Fact]
public async Task GetUserById_ReturnsUser()
{
    // Arrange
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    var mockUser = new User { UserId = 1, FirstName = "John" };
    
    mockUnitOfWork
        .Setup(uow => uow.Users.GetByIdAsync(1))
        .ReturnsAsync(mockUser);
    
    var service = new UserService(mockUnitOfWork.Object);
    
    // Act
    var result = await service.GetUserByIdAsync(1);
    
    // Assert
    Assert.Equal("John", result.FirstName);
}
```

### 3. **Transaction Management**
- Coordinate multiple operations in a single transaction
- Automatic rollback on failure
- Explicit transaction control

### 4. **Code Reusability**
- Generic repository reduces code duplication
- Common queries defined once
- Consistent API across all entities

### 5. **Maintainability**
- Centralized data access logic
- Easy to modify query behavior globally
- Clear boundaries between layers

---

## 📁 Project Structure

```
FitnessClassBookingWeb.DataAccess/
├── Data/
│   ├── ApplicationDbContext.cs       ✅ EF Core DbContext
│   └── DbInitializer.cs              ✅ Seed data
├── Repositories/                      ✅ NEW
│   ├── IRepository.cs                 ✅ Generic repository interface
│   └── Repository.cs                  ✅ Generic repository implementation
└── UnitOfWork/                        ✅ NEW
    ├── IUnitOfWork.cs                 ✅ Unit of Work interface
    └── UnitOfWork.cs                  ✅ Unit of Work implementation
```

---

## 🔄 Migration Guide

### Option 1: Update Existing Services (Recommended)

Update your existing services to use `IUnitOfWork` instead of `ApplicationDbContext`:

**Before:**
```csharp
public class GroupService : IGroupService
{
    private readonly ApplicationDbContext _context;

    public GroupService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Group?> GetByIdAsync(int id)
    {
        return await _context.Groups
            .Include(g => g.Coach)
            .FirstOrDefaultAsync(g => g.GroupId == id);
    }
}
```

**After:**
```csharp
public class GroupService : IGroupService
{
    private readonly IUnitOfWork _unitOfWork;

    public GroupService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Group?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Groups.FirstOrDefaultAsync(
            g => g.GroupId == id,
            g => g.Coach  // Include navigation property
        );
    }
}
```

### Option 2: Use Both (Gradual Migration)

You can use both `ApplicationDbContext` and `IUnitOfWork` during migration:

```csharp
public class GroupService : IGroupService
{
    private readonly ApplicationDbContext _context;  // Old way
    private readonly IUnitOfWork _unitOfWork;        // New way

    public GroupService(
        ApplicationDbContext context,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    // Migrate methods one by one to use _unitOfWork
}
```

---

## ⚠️ Important Notes

### 1. Include/ThenInclude Limitation
The generic repository uses `Include()` for eager loading. For `ThenInclude()`, you may need to extend the repository or use specific repositories:

```csharp
// Option A: Extend the repository
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserWithRolesAndPermissionsAsync(int userId);
}

// Option B: Use DbContext directly for complex queries
public async Task<User?> GetComplexUserDataAsync(int userId)
{
    // Fall back to DbContext for very complex queries
    return await _context.Users
        .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .FirstOrDefaultAsync(u => u.UserId == userId);
}
```

### 2. SaveChanges
Always call `SaveChangesAsync()` after modifications:

```csharp
// ❌ Wrong - changes not saved
await _unitOfWork.Users.AddAsync(user);

// ✅ Correct - changes saved to database
await _unitOfWork.Users.AddAsync(user);
await _unitOfWork.SaveChangesAsync();
```

### 3. Transactions
Transactions are optional and should be used when you need to ensure multiple operations succeed or fail together:

```csharp
// Without transaction - each SaveChanges is separate
await _unitOfWork.Users.AddAsync(user);
await _unitOfWork.SaveChangesAsync();  // Commits to DB

await _unitOfWork.UserRoles.AddAsync(userRole);
await _unitOfWork.SaveChangesAsync();  // Separate commit

// With transaction - all or nothing
await _unitOfWork.BeginTransactionAsync();
try
{
    await _unitOfWork.Users.AddAsync(user);
    await _unitOfWork.SaveChangesAsync();
    
    await _unitOfWork.UserRoles.AddAsync(userRole);
    await _unitOfWork.SaveChangesAsync();
    
    await _unitOfWork.CommitTransactionAsync();  // Both or neither
}
catch
{
    await _unitOfWork.RollbackTransactionAsync();
    throw;
}
```

---

## 🧪 Testing Example

```csharp
using Moq;
using Xunit;

public class GroupServiceTests
{
    [Fact]
    public async Task GetGroupById_WhenExists_ReturnsGroup()
    {
        // Arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var expectedGroup = new Group 
        { 
            GroupId = 1, 
            Name = "Yoga" 
        };
        
        mockUnitOfWork
            .Setup(uow => uow.Groups.GetByIdAsync(1))
            .ReturnsAsync(expectedGroup);
        
        var service = new GroupService(mockUnitOfWork.Object);
        
        // Act
        var result = await service.GetGroupByIdAsync(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Yoga", result.Name);
        mockUnitOfWork.Verify(
            uow => uow.Groups.GetByIdAsync(1), 
            Times.Once
        );
    }
}
```

---

## 🚀 Next Steps

1. **Build the project** to ensure everything compiles
2. **Update remaining services** to use Unit of Work
3. **Add unit tests** for services using mocked Unit of Work
4. **Consider creating specific repositories** for complex queries
5. **Review and refactor** existing code to use the new pattern

---

## 📚 Additional Resources

- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [Unit of Work Pattern](https://martinfowler.com/eaaCatalog/unitOfWork.html)
- [Generic Repository](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)

---

**Status:** ✅ COMPLETE  
**Pattern:** Repository + Unit of Work  
**Build:** Should compile successfully  
**Next:** Update services to use IUnitOfWork
