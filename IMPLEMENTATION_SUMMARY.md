# Fitness Class Booking Web App - Complete Implementation Guide

## 🎯 Project Overview

A full-stack web application for managing fitness class bookings with Angular frontend and ASP.NET Core backend.

## 📁 Project Structure

```
FitnessClassBookingWebApp/
├── FitnessClassBookingWebApp.Server/          # ASP.NET Core Backend
│   ├── Controllers/
│   │   ├── AdminController.cs                  # Admin management endpoints
│   │   ├── AuthController.cs                   # Authentication endpoints
│   │   ├── BookingsController.cs               # Booking management
│   │   ├── GroupsController.cs                 # Class management
│   │   ├── ReviewsController.cs                # Review management
│   │   └── SchedulesController.cs              # Schedule management
│   ├── Services/
│   │   ├── IAuthService.cs & AuthService.cs
│   │   ├── IBookingService.cs & BookingService.cs
│   │   ├── IGroupService.cs & GroupService.cs
│   │   ├── IReviewService.cs & ReviewService.cs
│   │   └── IScheduleService.cs & ScheduleService.cs
│   ├── Program.cs                              # App configuration
│   ├── appsettings.json                        # JWT & DB config
│   └── API_DOCUMENTATION.md
│
├── FitnessClassBookingWeb.Models/              # Shared Models
│   ├── DTOs/
│   │   ├── AuthResponseDto.cs
│   │   ├── BookingDto.cs
│   │   ├── GroupDto.cs
│   │   ├── LoginDto.cs
│   │   ├── RegisterDto.cs
│   │   ├── ReviewDto.cs
│   │   └── ScheduleDto.cs
│   ├── User.cs, Group.cs, Schedule.cs, etc.
│
├── FitnessClassBookingWeb.DataAccess/          # Data Layer
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── DbInitializer.cs
│   └── Migrations/
│
└── fitnessclassbookingwebapp.client/           # Angular Frontend
    └── src/app/
        ├── components/                          # UI Components
        │   ├── admin/
        │   ├── bookings/
        │   ├── groups/
        │   ├── home/
        │   ├── login/
        │   ├── navbar/
        │   └── register/
        ├── services/                            # API Services
        ├── guards/                              # Route Guards
        ├── interceptors/                        # HTTP Interceptor
        ├── models/                              # TypeScript Interfaces
        └── FRONTEND_DOCUMENTATION.md
```

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- Node.js 18+ & npm
- SQL Server (LocalDB or Full)
- Visual Studio 2022 or VS Code

### Backend Setup

1. **Update Connection String** (if needed)
   ```json
   // appsettings.json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=FitnessClassBookingDB;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

2. **Run Migrations**
   ```bash
   cd FitnessClassBookingWebApp.Server
   dotnet ef database update
   ```

3. **Run Backend**
   ```bash
   dotnet run
   ```

### Frontend Setup

1. **Install Dependencies**
   ```bash
   cd fitnessclassbookingwebapp.client
   npm install
   ```

2. **Run Frontend**
   ```bash
   npm start
   ```

3. **Access Application**
   - Frontend: `https://localhost:54827`
   - Backend API: `https://localhost:7xxx` (check console)

## 🔑 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `GET /api/auth/check-email/{email}` - Check email availability

### Groups (Classes)
- `GET /api/groups` - Get all classes
- `GET /api/groups/{id}` - Get class by ID
- `POST /api/groups` - Create class (Admin)
- `PUT /api/groups/{id}` - Update class (Admin)
- `DELETE /api/groups/{id}` - Delete class (Admin)

### Schedules
- `GET /api/schedules` - Get all schedules
- `GET /api/schedules/upcoming` - Get upcoming schedules
- `GET /api/schedules/group/{groupId}` - Get schedules for a class
- `POST /api/schedules` - Create schedule (Admin)
- `PUT /api/schedules/{id}` - Update schedule (Admin)
- `DELETE /api/schedules/{id}` - Delete schedule (Admin)

### Bookings
- `GET /api/bookings/user/{userId}` - Get user's bookings
- `POST /api/bookings` - Create booking
- `PATCH /api/bookings/{id}/cancel` - Cancel booking

### Reviews
- `GET /api/reviews/group/{groupId}` - Get reviews for a class
- `POST /api/reviews` - Create review
- `PUT /api/reviews/{id}` - Update review
- `DELETE /api/reviews/{id}` - Delete review

### Admin
- `GET /api/admin/statistics` - Get system statistics
- `GET /api/admin/users` - Get all users
- `PATCH /api/admin/users/{id}/toggle-active` - Toggle user status
- `GET /api/admin/rooms` - Get all rooms
- `POST /api/admin/rooms` - Create room
- `PUT /api/admin/rooms/{id}` - Update room

## 🎨 Frontend Features

### User Features
✅ Browse fitness classes  
✅ View class details with schedules and reviews  
✅ Register and login  
✅ Book classes  
✅ View and cancel bookings  
✅ Search and filter classes  

### Admin Features
✅ View system statistics  
✅ Manage users (view, activate/deactivate)  
✅ Manage classes (CRUD)  
✅ Manage schedules (CRUD)  
✅ Manage rooms (CRUD)  
✅ View all bookings  

## 🔒 Security

### Backend
- **JWT Authentication**: Token-based auth with 7-day expiration
- **Password Hashing**: SHA256 (consider upgrading to bcrypt)
- **HTTPS**: Enforced in production
- **CORS**: Configured for Angular app

### Frontend
- **Auth Guard**: Protects authenticated routes
- **Admin Guard**: Protects admin routes
- **Auth Interceptor**: Automatically adds JWT to requests
- **Form Validation**: Client-side validation
- **XSS Protection**: Angular built-in sanitization

## 📊 Database Schema

### Tables
- **Users** - User accounts
- **Roles** - User roles (Admin, Coach, User)
- **UserRoles** - Many-to-many relationship
- **Groups** - Fitness classes
- **Schedules** - Class schedules
- **Rooms** - Physical rooms
- **Bookings** - User bookings
- **Reviews** - Class reviews

### Default Roles
The system seeds with three roles:
1. **Admin** - Full system access
2. **Coach** - Manage own classes
3. **User** - Book classes, write reviews

## 🧪 Testing

### Test User Accounts
After database initialization, you can create test accounts:

1. **Admin User**
   - Register through `/register`
   - Manually assign Admin role via database or admin panel

2. **Regular User**
   - Register through `/register`
   - Automatically assigned "User" role

### Sample Data
The `DbInitializer` can be extended to seed:
- Sample rooms
- Sample groups
- Sample schedules
- Sample users

## 🎯 User Flows

### Booking a Class
1. Browse classes on `/groups`
2. Click on a class to view details
3. See upcoming schedules
4. Click "Book Class" (login if needed)
5. View booking in "My Bookings"

### Admin Managing System
1. Login as admin
2. Navigate to `/admin`
3. View statistics dashboard
4. Manage users, classes, schedules, rooms

## 🔄 API Response Format

### Success Response
```json
{
  "userId": 1,
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "roles": ["User"],
  "token": "eyJhbGc..."
}
```

### Error Response
```json
{
  "message": "Invalid email or password"
}
```

## 🌐 CORS Configuration

The backend accepts requests from the Angular dev server:
```csharp
// Already configured in Program.cs
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
```

## 📝 Environment Variables

### Recommended for Production

```json
// User Secrets or Azure Key Vault
{
  "Jwt": {
    "Key": "your-secret-key-min-32-characters",
    "Issuer": "FitnessClassBookingApp",
    "Audience": "FitnessClassBookingAppUsers"
  },
  "ConnectionStrings": {
    "DefaultConnection": "your-production-connection-string"
  }
}
```

## 🚧 Known Limitations

1. **Password Hashing**: Currently using SHA256 (should upgrade to bcrypt/Argon2)
2. **Refresh Tokens**: Not implemented (7-day expiration only)
3. **Email Verification**: Not implemented
4. **Password Reset**: Not implemented
5. **Image Upload**: Not implemented for classes
6. **Real-time Updates**: No WebSocket/SignalR integration

## 🔮 Future Enhancements

### Short Term
- [ ] Password reset functionality
- [ ] Email verification
- [ ] User profile editing
- [ ] Review editing UI
- [ ] Calendar view for schedules

### Medium Term
- [ ] Payment integration
- [ ] Booking waitlist
- [ ] Class capacity management
- [ ] Push notifications
- [ ] Coach dashboard

### Long Term
- [ ] Mobile app (React Native/Flutter)
- [ ] Analytics dashboard
- [ ] Automated reminders
- [ ] Multi-location support
- [ ] Membership tiers

## 📚 Documentation Files

1. **API_DOCUMENTATION.md** - Backend API reference
2. **FRONTEND_DOCUMENTATION.md** - Angular app structure
3. **IMPLEMENTATION_SUMMARY.md** - This file

## 🤝 Contributing

### Adding a New Feature

1. **Backend**
   - Add model to `FitnessClassBookingWeb.Models`
   - Add DTO to `Models/DTOs`
   - Create service interface and implementation
   - Create controller with endpoints
   - Register service in `Program.cs`

2. **Frontend**
   - Add model to `models/`
   - Create service in `services/`
   - Create component(s) in `components/`
   - Add to `app-module.ts`
   - Add route to `app-routing-module.ts`

### Code Style
- **Backend**: Follow C# naming conventions
- **Frontend**: Follow Angular style guide
- **Components**: One component per feature
- **Services**: One service per entity

## ⚙️ Build & Deployment

### Development Build
```bash
# Backend
dotnet build

# Frontend
npm run build
```

### Production Build
```bash
# Backend
dotnet publish -c Release

# Frontend
ng build --configuration production
```

### Deployment Checklist
- [ ] Update JWT secret in production
- [ ] Update connection string
- [ ] Enable HTTPS
- [ ] Configure CORS for production URL
- [ ] Set up logging
- [ ] Configure error handling
- [ ] Set up database backups
- [ ] Enable Application Insights (Azure)

## 📞 Support & Resources

- **Angular Docs**: https://angular.dev
- **ASP.NET Core Docs**: https://docs.microsoft.com/aspnet/core
- **Entity Framework Core**: https://docs.microsoft.com/ef/core

## ✅ Checklist for New Developers

- [ ] Clone repository
- [ ] Install prerequisites (.NET 10, Node 18+, SQL Server)
- [ ] Update connection string
- [ ] Run database migrations
- [ ] Install npm packages
- [ ] Start backend server
- [ ] Start frontend server
- [ ] Create test user account
- [ ] Explore API documentation
- [ ] Review frontend components

---

**Last Updated**: 2025  
**Version**: 1.0.0  
**Framework**: .NET 10 + Angular 21
