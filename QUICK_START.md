# 🚀 Quick Start Guide

## First Time Setup (5 minutes)

### 1. Start Backend API
```bash
cd FitnessClassBookingWebApp.Server
dotnet run
```
✅ Backend running on https://localhost:xxxx

### 2. Start Frontend (New Terminal)
```bash
cd fitnessclassbookingwebapp.client
npm start
```
✅ Frontend running on https://localhost:54827

### 3. Open Browser
Navigate to: **https://localhost:54827**

---

## Quick Test Flow

### Register a New Account
1. Click **"Sign Up"** in navbar
2. Fill in registration form
3. Click **"Register"**
4. You're automatically logged in!

### Browse & Book a Class
1. Click **"Classes"** in navbar
2. Click on any class to view details
3. Click **"Book Class"** on an upcoming schedule
4. View your booking in **"My Bookings"**

### View Your Bookings
1. Click **"My Bookings"** in navbar
2. See all your bookings with status
3. Cancel upcoming bookings if needed

---

## Admin Access

To test admin features:

### Option 1: Assign Admin Role via Database
```sql
-- Connect to FitnessClassBookingDB
-- Find your user
SELECT * FROM Users WHERE Email = 'your@email.com'

-- Add Admin role (RoleId 1 is Admin)
INSERT INTO UserRoles (UserId, RoleId)
VALUES (YourUserId, 1)
```

### Option 2: Register with Admin Email
Modify `DbInitializer.cs` to auto-assign admin to specific emails.

### Access Admin Dashboard
1. Login with admin account
2. Click **"Admin"** in navbar
3. View statistics and manage system

---

## Common Commands

### Backend
```bash
# Run server
dotnet run

# Build
dotnet build

# Run migrations
dotnet ef database update

# Create new migration
dotnet ef migrations add MigrationName
```

### Frontend
```bash
# Install dependencies
npm install

# Start dev server
npm start

# Build for production
npm run build

# Run tests
npm test
```

---

## Troubleshooting

### Database Connection Error
**Problem**: "Cannot connect to SQL Server"

**Solution**:
1. Check SQL Server is running
2. Update connection string in `appsettings.json`
3. Run migrations: `dotnet ef database update`

### CORS Error
**Problem**: "No 'Access-Control-Allow-Origin' header"

**Solution**: Ensure backend is running before starting frontend

### JWT Token Error
**Problem**: "Unauthorized 401"

**Solution**:
1. Login again to get fresh token
2. Check `localStorage` has `currentUser` with token
3. Verify JWT configuration in `appsettings.json`

### Angular Compilation Error
**Problem**: "Cannot find module"

**Solution**:
```bash
cd fitnessclassbookingwebapp.client
rm -rf node_modules
rm package-lock.json
npm install
```

---

## Default Ports

| Service | Port | URL |
|---------|------|-----|
| Frontend | 54827 | https://localhost:54827 |
| Backend | Auto | Check console output |
| SQL Server | 1433 | localhost |

---

## Project Structure Quick Reference

```
Backend API Endpoints: /api/
├── /auth          → Authentication
├── /groups        → Fitness Classes
├── /schedules     → Class Schedules
├── /bookings      → User Bookings
├── /reviews       → Class Reviews
└── /admin         → Admin Functions

Frontend Routes:
├── /              → Home Page
├── /login         → Login Page
├── /register      → Sign Up Page
├── /groups        → Browse Classes
├── /groups/:id    → Class Details
├── /bookings      → My Bookings
└── /admin         → Admin Dashboard
```

---

## Next Steps After Setup

1. ✅ Create test user account
2. ✅ Browse classes
3. ✅ Make a booking
4. ✅ Write a review
5. ✅ Access admin dashboard (if admin)
6. 📖 Read full documentation in:
   - `API_DOCUMENTATION.md`
   - `FRONTEND_DOCUMENTATION.md`
   - `IMPLEMENTATION_SUMMARY.md`

---

## Need Help?

- **Backend Issues**: Check `API_DOCUMENTATION.md`
- **Frontend Issues**: Check `FRONTEND_DOCUMENTATION.md`
- **Overall Architecture**: Check `IMPLEMENTATION_SUMMARY.md`
- **Database Issues**: Check migrations in `DataAccess/Migrations/`

---

**Happy Coding! 💻🏋️**
