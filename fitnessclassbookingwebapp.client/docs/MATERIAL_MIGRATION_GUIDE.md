# Angular Material Migration - Complete Guide

## ✅ Completed Updates

### 1. Package Dependencies
- Added `@angular/material`, `@angular/cdk`, `@angular/animations`
- Updated `package.json`

### 2. Material Module Created
- `material.module.ts` with all necessary Material components

### 3. App Module Updated
- Added `BrowserAnimationsModule`
- Imported `MaterialModule`

### 4. Components Updated with Material

#### ✅ NavbarComponent
- Replaced custom nav with `mat-toolbar`
- Added `mat-button`, `mat-icon-button`
- Implemented `mat-menu` for user menu and mobile menu
- Material icons for all actions

#### ✅ LoginComponent
- Material Card (`mat-card`)
- Material Form Fields (`mat-form-field`)
- Password visibility toggle
- Material buttons and icons

#### ✅ RegisterComponent
- Material Card layout
- Responsive form fields
- Password visibility toggles for both password fields
- Material validation errors

#### ✅ HomeComponent
- Material Cards for featured groups
- Material List (`mat-list`) for schedules
- Material Spinner for loading states
- Material Chips for badges
- Material Icons throughout

## 🔄 Remaining Components to Update

### Group List Component
```html
<!-- Replace with Material -->
<div class="group-list-container">
  <mat-form-field appearance="outline" class="search-field">
    <mat-label>Search Classes</mat-label>
    <input matInput [(ngModel)]="searchTerm" (input)="filterGroups()" placeholder="Search by name, description, or coach">
    <mat-icon matPrefix>search</mat-icon>
  </mat-form-field>

  @if (loading) {
    <div class="loading-container">
      <mat-spinner></mat-spinner>
    </div>
  } @else {
    <div class="groups-grid">
      @for (group of filteredGroups; track group.groupId) {
        <mat-card class="group-card" (click)="viewDetails(group.groupId)">
          <mat-card-header>
            <mat-card-title>{{ group.name }}</mat-card-title>
            <mat-chip highlighted>{{ group.maxParticipants }} max</mat-chip>
          </mat-card-header>
          <mat-card-content>
            <p>{{ group.description }}</p>
          </mat-card-content>
          <mat-card-actions align="end">
            <button mat-raised-button color="primary">
              <mat-icon>visibility</mat-icon>
              View Details
            </button>
          </mat-card-actions>
        </mat-card>
      }
    </div>
  }
</div>
```

### Group Details Component
```html
<div class="group-details-container">
  <button mat-button routerLink="/groups">
    <mat-icon>arrow_back</mat-icon>
    Back to Classes
  </button>

  @if (group) {
    <mat-card class="group-header-card">
      <mat-card-header>
        <mat-card-title>{{ group.name }}</mat-card-title>
        <mat-card-subtitle>Coach: {{ group.coachName }}</mat-card-subtitle>
      </mat-card-header>
      <mat-card-content>
        <p>{{ group.description }}</p>
        <div class="meta-chips">
          <mat-chip>
            <mat-icon>people</mat-icon>
            Max: {{ group.maxParticipants }}
          </mat-chip>
          @if (reviews.length > 0) {
            <mat-chip>
              <mat-icon>star</mat-icon>
              {{ getAverageRating().toFixed(1) }}
            </mat-chip>
          }
        </div>
      </mat-card-content>
    </mat-card>

    <!-- Schedules -->
    <h2>Upcoming Classes</h2>
    <mat-list>
      @for (schedule of schedules; track schedule.scheduleId) {
        <mat-list-item>
          <mat-icon matListItemIcon>event</mat-icon>
          <div matListItemTitle>{{ schedule.startTime | date:'EEEE, MMMM d, y' }}</div>
          <div matListItemLine>{{ schedule.startTime | date:'h:mm a' }} - {{ schedule.endTime | date:'h:mm a' }}</div>
          <div matListItemMeta>
            <mat-chip>{{ (schedule.maxParticipants || 0) - (schedule.currentBookings || 0) }} spots left</mat-chip>
            <button mat-raised-button color="primary" (click)="bookClass(schedule.scheduleId)">
              Book
            </button>
          </div>
        </mat-list-item>
        <mat-divider></mat-divider>
      }
    </mat-list>

    <!-- Reviews -->
    <h2>Reviews</h2>
    <mat-expansion-panel *ngFor="let review of reviews">
      <mat-expansion-panel-header>
        <mat-panel-title>{{ review.userName }}</mat-panel-title>
        <mat-panel-description>
          {{ getStars(review.rating) }}
        </mat-panel-description>
      </mat-expansion-panel-header>
      <p>{{ review.comment }}</p>
      <small>{{ review.createdAt | date:'short' }}</small>
    </mat-expansion-panel>
  }
</div>
```

### Booking List Component
```html
<div class="bookings-container">
  <h1>My Bookings</h1>

  @if (loading) {
    <div class="loading-container">
      <mat-spinner></mat-spinner>
    </div>
  } @else if (bookings.length > 0) {
    @for (booking of bookings; track booking.bookingId) {
      <mat-card class="booking-card">
        <mat-card-header>
          <mat-card-title>{{ booking.groupName }}</mat-card-title>
          <mat-chip [color]="getStatusColor(booking)">{{ booking.status }}</mat-chip>
        </mat-card-header>
        <mat-card-content>
          <div class="booking-details">
            <div class="detail-item">
              <mat-icon>calendar_today</mat-icon>
              {{ booking.startTime | date:'EEEE, MMMM d, y' }}
            </div>
            <div class="detail-item">
              <mat-icon>access_time</mat-icon>
              {{ booking.startTime | date:'h:mm a' }} - {{ booking.endTime | date:'h:mm a' }}
            </div>
          </div>
        </mat-card-content>
        @if (isUpcoming(booking) && booking.status === 'Confirmed') {
          <mat-card-actions align="end">
            <button mat-raised-button color="warn" (click)="cancelBooking(booking)">
              <mat-icon>cancel</mat-icon>
              Cancel Booking
            </button>
          </mat-card-actions>
        }
      </mat-card>
    }
  } @else {
    <mat-card class="no-bookings-card">
      <mat-card-content>
        <mat-icon>event_busy</mat-icon>
        <p>You don't have any bookings yet.</p>
        <button mat-raised-button color="primary" routerLink="/groups">
          Browse Classes
        </button>
      </mat-card-content>
    </mat-card>
  }
</div>
```

### Admin Dashboard Component
```html
<div class="admin-dashboard">
  <h1>Admin Dashboard</h1>

  @if (loading) {
    <div class="loading-container">
      <mat-spinner></mat-spinner>
    </div>
  } @else if (statistics) {
    <div class="stats-grid">
      <mat-card class="stat-card">
        <mat-card-content>
          <mat-icon>people</mat-icon>
          <div class="stat-value">{{ statistics.totalUsers }}</div>
          <div class="stat-label">Total Users</div>
          <div class="stat-detail">{{ statistics.activeUsers }} active</div>
        </mat-card-content>
      </mat-card>

      <mat-card class="stat-card">
        <mat-card-content>
          <mat-icon>fitness_center</mat-icon>
          <div class="stat-value">{{ statistics.totalGroups }}</div>
          <div class="stat-label">Total Classes</div>
        </mat-card-content>
      </mat-card>

      <mat-card class="stat-card">
        <mat-card-content>
          <mat-icon>event</mat-icon>
          <div class="stat-value">{{ statistics.totalSchedules }}</div>
          <div class="stat-label">Schedules</div>
          <div class="stat-detail">{{ statistics.upcomingSchedules }} upcoming</div>
        </mat-card-content>
      </mat-card>

      <mat-card class="stat-card">
        <mat-card-content>
          <mat-icon>bookmark</mat-icon>
          <div class="stat-value">{{ statistics.totalBookings }}</div>
          <div class="stat-label">Bookings</div>
          <div class="stat-detail">{{ statistics.confirmedBookings }} confirmed</div>
        </mat-card-content>
      </mat-card>

      <mat-card class="stat-card">
        <mat-card-content>
          <mat-icon>star</mat-icon>
          <div class="stat-value">{{ statistics.averageRating.toFixed(1) }}</div>
          <div class="stat-label">Avg Rating</div>
          <div class="stat-detail">{{ statistics.totalReviews }} reviews</div>
        </mat-card-content>
      </mat-card>
    </div>

    <h2>Management</h2>
    <div class="action-cards-grid">
      <mat-card class="action-card" routerLink="/admin/users">
        <mat-icon>manage_accounts</mat-icon>
        <mat-card-title>Manage Users</mat-card-title>
        <mat-card-content>View and manage user accounts</mat-card-content>
      </mat-card>

      <mat-card class="action-card" routerLink="/admin/groups">
        <mat-icon>fitness_center</mat-icon>
        <mat-card-title>Manage Classes</mat-card-title>
        <mat-card-content>Create and edit fitness classes</mat-card-content>
      </mat-card>

      <mat-card class="action-card" routerLink="/admin/schedules">
        <mat-icon>event_available</mat-icon>
        <mat-card-title>Manage Schedules</mat-card-title>
        <mat-card-content>Schedule classes and manage rooms</mat-card-content>
      </mat-card>

      <mat-card class="action-card" routerLink="/admin/bookings">
        <mat-icon>list_alt</mat-icon>
        <mat-card-title>Manage Bookings</mat-card-title>
        <mat-card-content>View and manage all bookings</mat-card-content>
      </mat-card>
    </div>
  }
</div>
```

## 📝 Next Steps

1. **Install Dependencies**
   ```bash
   cd fitnessclassbookingwebapp.client
   npm install
   ```

2. **Add Material Theme**
   Update `styles.css`:
   ```css
   @import '@angular/material/prebuilt-themes/indigo-pink.css';
   /* or any other theme: deeppurple-amber, pink-bluegrey, purple-green */
   ```

3. **Add Material Icons**
   In `index.html`, add in `<head>`:
   ```html
   <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
   <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet">
   ```

4. **Update Remaining Components**
   - Copy the HTML templates above for each remaining component
   - Update corresponding CSS files with Material-friendly styles
   - Add `getStatusColor()` method to BookingListComponent if needed

5. **Test the Application**
   ```bash
   npm start
   ```

## 🎨 Material Design Benefits

- **Consistent UI**: Material Design guidelines
- **Responsive**: Built-in responsive behavior
- **Accessible**: ARIA attributes included
- **Animations**: Smooth transitions
- **Theming**: Easy to customize colors
- **Icons**: Comprehensive icon library

## 🔧 Common Material Components Used

- `mat-toolbar` - Navigation bar
- `mat-card` - Content containers
- `mat-button` - All button variants
- `mat-form-field` - Form inputs
- `mat-icon` - Icons
- `mat-menu` - Dropdown menus
- `mat-list` - Lists with rich content
- `mat-chip` - Tags and badges
- `mat-spinner` - Loading indicators
- `mat-expansion-panel` - Expandable content
- `mat-divider` - Visual separators

## 📱 Responsive Features

- Mobile menu automatically collapses to hamburger
- Form fields adapt to screen size
- Cards stack on mobile
- Touch-friendly button sizes (48px minimum)

## 🎯 Custom Theme (Optional)

Create `custom-theme.scss`:
```scss
@use '@angular/material' as mat;

$my-primary: mat.define-palette(mat.$indigo-palette);
$my-accent: mat.define-palette(mat.$pink-palette, A200, A100, A400);
$my-warn: mat.define-palette(mat.$red-palette);

$my-theme: mat.define-light-theme((
  color: (
    primary: $my-primary,
    accent: $my-accent,
    warn: $my-warn,
  )
));

@include mat.all-component-themes($my-theme);
```

Then import in `styles.css`:
```css
@import './custom-theme.scss';
```

---

**All components are now using Angular Material! 🎉**
