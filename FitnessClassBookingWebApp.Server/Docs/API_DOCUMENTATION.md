# Fitness Class Booking Web App - API Documentation

## Overview
This project has been updated with complete authentication and CRUD operations for managing a fitness class booking system.

## Changes Made

### 1. Deleted Weather Forecast
- Removed `WeatherForecast.cs`
- Removed `WeatherForecastController.cs`

### 2. Created Data Transfer Objects (DTOs)
Located in `FitnessClassBookingWeb.Models\DTOs\`:
- `LoginDto.cs` - Login credentials
- `RegisterDto.cs` - User registration data
- `AuthResponseDto.cs` - Authentication response with JWT token
- `GroupDto.cs` - Fitness group information
- `ScheduleDto.cs` - Class schedule information
- `BookingDto.cs` - Booking details
- `ReviewDto.cs` - Review information

### 3. Created Service Interfaces
Located in `FitnessClassBookingWebApp.Server\Services\`:
- `IAuthService.cs` - Authentication service interface
- `IGroupService.cs` - Group management interface
- `IScheduleService.cs` - Schedule management interface
- `IBookingService.cs` - Booking management interface
- `IReviewService.cs` - Review management interface

### 4. Created Service Implementations
- `AuthService.cs` - Handles user registration, login, and JWT token generation
- `GroupService.cs` - CRUD operations for fitness groups
- `ScheduleService.cs` - CRUD operations for class schedules
- `BookingService.cs` - Booking creation, cancellation, and management
- `ReviewService.cs` - CRUD operations for reviews

### 5. Created API Controllers
Located in `FitnessClassBookingWebApp.Server\Controllers\`:

#### AuthController
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `GET /api/auth/check-email/{email}` - Check if email exists

#### GroupsController
- `GET /api/groups` - Get all groups
- `GET /api/groups/{id}` - Get group by ID
- `POST /api/groups` - Create new group
- `PUT /api/groups/{id}` - Update group
- `DELETE /api/groups/{id}` - Delete group

#### SchedulesController
- `GET /api/schedules` - Get all schedules
- `GET /api/schedules/{id}` - Get schedule by ID
- `GET /api/schedules/group/{groupId}` - Get schedules by group
- `GET /api/schedules/upcoming` - Get upcoming schedules
- `POST /api/schedules` - Create new schedule
- `PUT /api/schedules/{id}` - Update schedule
- `DELETE /api/schedules/{id}` - Delete schedule

#### BookingsController
- `GET /api/bookings` - Get all bookings
- `GET /api/bookings/{id}` - Get booking by ID
- `GET /api/bookings/user/{userId}` - Get bookings by user
- `GET /api/bookings/schedule/{scheduleId}` - Get bookings by schedule
- `POST /api/bookings` - Create new booking
- `PATCH /api/bookings/{id}/cancel` - Cancel booking
- `PATCH /api/bookings/{id}/status` - Update booking status

#### ReviewsController
- `GET /api/reviews` - Get all reviews
- `GET /api/reviews/{id}` - Get review by ID
- `GET /api/reviews/group/{groupId}` - Get reviews by group
- `GET /api/reviews/user/{userId}` - Get reviews by user
- `POST /api/reviews` - Create new review
- `PUT /api/reviews/{id}` - Update review
- `DELETE /api/reviews/{id}` - Delete review

#### AdminController
- `GET /api/admin/users` - Get all users
- `GET /api/admin/users/{id}` - Get user by ID
- `PATCH /api/admin/users/{id}/toggle-active` - Toggle user active status
- `GET /api/admin/roles` - Get all roles
- `POST /api/admin/users/{userId}/roles/{roleId}` - Assign role to user
- `DELETE /api/admin/users/{userId}/roles/{roleId}` - Remove role from user
- `GET /api/admin/rooms` - Get all rooms
- `GET /api/admin/rooms/{id}` - Get room by ID
- `POST /api/admin/rooms` - Create new room
- `PUT /api/admin/rooms/{id}` - Update room
- `DELETE /api/admin/rooms/{id}` - Delete room
- `GET /api/admin/statistics` - Get system statistics

### 6. Updated Configuration

#### Program.cs
- Added service registrations for all business services
- Configured JWT Bearer authentication
- Added authentication middleware

#### appsettings.json
- Added JWT configuration section with Key, Issuer, and Audience

#### Project File
- Added `Microsoft.AspNetCore.Authentication.JwtBearer` package (v10.0.3)

## Authentication
The API uses JWT (JSON Web Token) authentication. To use protected endpoints:

1. Register a new user via `/api/auth/register`
2. Login via `/api/auth/login` to receive a JWT token
3. Include the token in the Authorization header: `Bearer {token}`

## Security Notes
- Passwords are hashed using SHA256 (Note: In production, consider using bcrypt or Argon2)
- JWT tokens expire after 7 days
- The JWT secret key should be stored in Azure Key Vault or User Secrets for production

## Next Steps
Consider implementing:
- Role-based authorization attributes on controllers
- Input validation and error handling middleware
- Rate limiting for authentication endpoints
- Email verification for new users
- Password reset functionality
- Refresh tokens for improved security
