import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Booking, CreateBookingRequest } from '../models/booking.model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private readonly API_URL = '/api/bookings';

  constructor(private http: HttpClient) {}

  getAllBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(this.API_URL);
  }

  getBookingById(id: number): Observable<Booking> {
    return this.http.get<Booking>(`${this.API_URL}/${id}`);
  }

  getBookingsByUser(userId: number): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.API_URL}/user/${userId}`);
  }

  getBookingsBySchedule(scheduleId: number): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.API_URL}/schedule/${scheduleId}`);
  }

  createBooking(request: CreateBookingRequest): Observable<Booking> {
    return this.http.post<Booking>(this.API_URL, request);
  }

  cancelBooking(id: number, userId: number): Observable<void> {
    return this.http.patch<void>(`${this.API_URL}/${id}/cancel`, { userId });
  }

  updateBookingStatus(id: number, status: string): Observable<void> {
    return this.http.patch<void>(`${this.API_URL}/${id}/status`, { status });
  }
}
