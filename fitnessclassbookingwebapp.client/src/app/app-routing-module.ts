import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { GroupListComponent } from './components/groups/group-list.component';
import { GroupReviewComponent } from './components/groups/group-review/group-review.component';
import { GroupReviewUpdateComponent } from './components/groups/group-review-update/group-review-update.component';
import { GroupDetailsComponent } from './components/groups/group-details.component';
import { BookingListComponent } from './components/bookings/booking-list.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard.component';
import { AdminLayoutComponent } from './components/admin/admin-layout.component';
import { AdminUsersComponent } from './components/admin/admin-users.component';
import { AdminGroupsComponent } from './components/admin/admin-groups.component';
import { AdminSchedulesComponent } from './components/admin/admin-schedules.component';
import { AdminBookingsComponent } from './components/admin/admin-bookings.component';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'groups', component: GroupListComponent },
  { path: 'groups/:id/review', component: GroupReviewComponent, canActivate: [authGuard] },
  { path: 'groups/:id/review/update', component: GroupReviewUpdateComponent, canActivate: [authGuard] },
  { path: 'groups/:id', component: GroupDetailsComponent },
  { path: 'bookings', component: BookingListComponent, canActivate: [authGuard] },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [adminGuard],
    children: [
      { path: '', component: AdminDashboardComponent },
      { path: 'users', component: AdminUsersComponent },
      { path: 'groups', component: AdminGroupsComponent },
      { path: 'schedules', component: AdminSchedulesComponent },
      { path: 'bookings', component: AdminBookingsComponent }
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
