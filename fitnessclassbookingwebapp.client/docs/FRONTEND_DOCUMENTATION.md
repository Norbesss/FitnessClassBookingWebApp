# Fitness Class Booking - Angular Frontend

## Overview
This is the Angular frontend application for the Fitness Class Booking system. It provides a user-friendly interface for browsing fitness classes, making bookings, writing reviews, and administrative management.

## Project Structure

```
src/app/
├── components/
│   ├── admin/
│   │   ├── admin-dashboard.component.ts/html/css
│   ├── bookings/
│   │   ├── booking-list.component.ts/html/css
│   ├── groups/
│   │   ├── group-list.component.ts/html/css
│   │   └── group-details.component.ts/html/css
│   ├── home/
│   │   └── home.component.ts/html/css
│   ├── login/
│   │   └── login.component.ts/html/css
│   ├── navbar/
│   │   └── navbar.component.ts/html/css
│   └── register/
│       └── register.component.ts/html/css
├── guards/
│   ├── auth.guard.ts
│   └── admin.guard.ts
├── interceptors/
│   └── auth.interceptor.ts
├── models/
│   ├── admin.model.ts
│   ├── auth.model.ts
│   ├── booking.model.ts
│   ├── group.model.ts
│   ├── review.model.ts
│   └── schedule.model.ts
├── services/
│   ├── admin.service.ts
│   ├── auth.service.ts
│   ├── booking.service.ts
│   ├── group.service.ts
│   ├── review.service.ts
│   └── schedule.service.ts
├── app-module.ts
├── app-routing-module.ts
├── app.ts
└── app.html
```

## Features Implemented

### 🔐 Authentication
- **Login Component**: User authentication with JWT token
- **Register Component**: New user registration with form validation
- **Auth Service**: Handles login, registration, and token management
- **Auth Interceptor**: Automatically adds JWT token to HTTP requests
- **Auth Guard**: Protects routes requiring authentication

### 🏋️ Classes (Groups)
- **Group List**: Browse all available fitness classes
- **Group Details**: View class details, schedules, and reviews
- **Search**: Filter classes by name, description, or coach

### 📅 Bookings
- **My Bookings**: View all user bookings
- **Book Class**: Book available class schedules
- **Cancel Booking**: Cancel upcoming bookings
- **Status Tracking**: Visual indicators for confirmed, cancelled, and completed bookings

### 👑 Admin Dashboard
- **Statistics Overview**: System-wide statistics
- **User Management**: View and manage users (placeholder for full implementation)
- **Class Management**: Links to manage groups and schedules
- **Admin Guard**: Restricts admin routes to users with Admin role

### 🎨 UI/UX Features
- **Responsive Design**: Mobile-friendly layout
- **Navigation Bar**: Dynamic navigation with user authentication state
- **Loading States**: Visual feedback during data fetching
- **Error Handling**: User-friendly error messages
- **Form Validation**: Real-time form validation with error messages

## Key Technologies

- **Angular 21.2.0**: Latest Angular framework
- **RxJS**: Reactive programming for async operations
- **TypeScript**: Type-safe development
- **FormsModule & ReactiveFormsModule**: Form handling
- **Angular Router**: Client-side routing
- **HttpClient**: API communication

## Services

### AuthService
```typescript
- register(registerDto: RegisterDto): Observable<AuthResponseDto>
- login(loginDto: LoginDto): Observable<AuthResponseDto>
- logout(): void
- isAuthenticated(): boolean
- isAdmin(): boolean
- getCurrentUser(): AuthResponseDto | null
```

### GroupService
```typescript
- getAllGroups(): Observable<Group[]>
- getGroupById(id: number): Observable<Group>
- createGroup(groupDto: GroupDto): Observable<Group>
- updateGroup(id: number, groupDto: GroupDto): Observable<Group>
- deleteGroup(id: number): Observable<void>
```

### BookingService
```typescript
- getBookingsByUser(userId: number): Observable<Booking[]>
- createBooking(request: CreateBookingRequest): Observable<Booking>
- cancelBooking(id: number, userId: number): Observable<void>
```

### ScheduleService
```typescript
- getAllSchedules(): Observable<Schedule[]>
- getSchedulesByGroup(groupId: number): Observable<Schedule[]>
- getUpcomingSchedules(): Observable<Schedule[]>
```

### ReviewService
```typescript
- getReviewsByGroup(groupId: number): Observable<Review[]>
- createReview(reviewDto: ReviewDto): Observable<Review>
```

### AdminService
```typescript
- getAllUsers(): Observable<User[]>
- getStatistics(): Observable<Statistics>
- getAllRooms(): Observable<Room[]>
```

## Routes

| Path | Component | Guard | Description |
|------|-----------|-------|-------------|
| `/` | HomeComponent | - | Landing page with featured classes |
| `/login` | LoginComponent | - | User login |
| `/register` | RegisterComponent | - | User registration |
| `/groups` | GroupListComponent | - | Browse all classes |
| `/groups/:id` | GroupDetailsComponent | - | Class details and booking |
| `/bookings` | BookingListComponent | authGuard | User's bookings |
| `/admin` | AdminDashboardComponent | adminGuard | Admin dashboard |

## Route Guards

### authGuard
- Checks if user is authenticated
- Redirects to `/login` if not authenticated
- Stores return URL for post-login redirect

### adminGuard
- Checks if user is authenticated AND has Admin role
- Redirects to `/login` if not authenticated
- Redirects to `/` if authenticated but not admin

## HTTP Interceptor

### authInterceptor
- Automatically attaches JWT token to all HTTP requests
- Reads token from localStorage
- Adds `Authorization: Bearer {token}` header

## Models/Interfaces

All TypeScript interfaces match the backend DTOs:

- **AuthResponseDto**: User info with JWT token
- **Group**: Fitness class information
- **Schedule**: Class schedule with time and room
- **Booking**: User booking information
- **Review**: User review with rating
- **Statistics**: Admin dashboard statistics

## Running the Application

```bash
# Install dependencies
npm install

# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test
```

The app will run on `https://localhost:54827` (configured in proxy)

## Environment Configuration

The API calls are proxied through Angular's development server. The backend API should run on the configured port in `Program.cs`.

## Future Enhancements

### Planned Features
- ✅ Review creation and editing
- ✅ User profile management
- ✅ Admin management pages (users, rooms, schedules)
- ✅ Real-time availability updates
- ✅ Advanced search and filtering
- ✅ Class schedule calendar view
- ✅ Push notifications for bookings
- ✅ Payment integration

### Current Implementation Status
- ✅ Authentication (Login/Register)
- ✅ Browse Classes
- ✅ View Class Details
- ✅ Book Classes
- ✅ View My Bookings
- ✅ Cancel Bookings
- ✅ View Reviews
- ✅ Admin Dashboard (Statistics)
- ⏳ Admin User Management (UI created, full CRUD pending)
- ⏳ Admin Room Management (UI created, full CRUD pending)
- ⏳ Create Reviews (Service ready, UI pending)
- ⏳ User Profile Page

## Styling

The application uses a custom CSS design system with:
- **Primary Color**: #667eea (Purple-Blue)
- **Secondary Color**: #764ba2 (Purple)
- **Background**: #f5f7fa (Light Gray)
- **Card Shadows**: Subtle elevation effects
- **Responsive Breakpoint**: 768px

All components have dedicated CSS files for scoped styling.

## Security

- JWT tokens stored in localStorage
- Auth interceptor adds token to requests
- Guards protect sensitive routes
- Password validation (min 6 characters)
- Email format validation
- XSS protection through Angular's built-in sanitization

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## Contributing

When adding new components:
1. Create component in appropriate folder under `components/`
2. Add to `app-module.ts` declarations
3. Add route to `app-routing-module.ts`
4. Create corresponding service if needed
5. Add models/interfaces to `models/`

## Notes

- All dates are displayed in user's local timezone
- Star ratings display as ★☆ characters
- Forms use reactive forms with validation
- All API calls return Observables (subscribe in components)
