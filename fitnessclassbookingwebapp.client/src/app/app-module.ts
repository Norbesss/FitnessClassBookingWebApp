import { HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { authInterceptor } from './interceptors/auth.interceptor';
import { MaterialModule } from './material.module';

// Components
import { NavbarComponent } from './components/navbar/navbar.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { GroupListComponent } from './components/groups/group-list.component';
import { GroupDetailsComponent } from './components/groups/group-details.component';
import { BookingListComponent } from './components/bookings/booking-list.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard.component';

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
    AdminDashboardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    AppRoutingModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors([authInterceptor]))
  ],
  bootstrap: [App]
})
export class AppModule { }
