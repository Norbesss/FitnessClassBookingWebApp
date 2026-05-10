import { HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { authInterceptor } from './interceptors/auth.interceptor';
import { MaterialModule } from './material.module';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { GroupListComponent } from './components/groups/group-list.component';
import { GroupDetailsComponent } from './components/groups/group-details.component';
import { BookingListComponent } from './components/bookings/booking-list.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard.component';
import { AdminLayoutComponent } from './components/admin/admin-layout.component';
import { AdminUsersComponent } from './components/admin/admin-users.component';
import { AdminGroupsComponent } from './components/admin/admin-groups.component';
import { AdminSchedulesComponent } from './components/admin/admin-schedules.component';
import { AdminBookingsComponent } from './components/admin/admin-bookings.component';
import { GroupReviewComponent } from './components/groups/group-review/group-review.component';
import { GroupReviewUpdateComponent } from './components/groups/group-review-update/group-review-update.component';

@NgModule({
  declarations: [
    App,
    NavbarComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    GroupListComponent,
    GroupDetailsComponent,
    BookingListComponent,
    AdminDashboardComponent,
    AdminLayoutComponent,
    AdminUsersComponent,
    AdminGroupsComponent,
    AdminSchedulesComponent,
    AdminBookingsComponent,
    GroupReviewComponent,
    GroupReviewUpdateComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    AppRoutingModule,
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors([authInterceptor])),
  ],
  bootstrap: [App],
})
export class AppModule {}
