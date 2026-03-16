import { Component, OnInit } from '@angular/core';
import { BookingService } from '../../services/booking.service';
import { AuthService } from '../../services/auth.service';
import { Booking } from '../../models/booking.model';

@Component({
  selector: 'app-booking-list',
  templateUrl: './booking-list.component.html',
  styleUrl: './booking-list.component.css',
  standalone: false
})
export class BookingListComponent implements OnInit {
  bookings: Booking[] = [];
  loading = true;
  currentUserId: number | null = null;

  constructor(
    private bookingService: BookingService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.currentUserId = user.userId;
      this.loadBookings();
    }
  }

  loadBookings(): void {
    if (this.currentUserId) {
      this.bookingService.getBookingsByUser(this.currentUserId).subscribe({
        next: (bookings) => {
          this.bookings = bookings.sort((a, b) => 
            new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
          );
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading bookings:', error);
          this.loading = false;
        }
      });
    }
  }

  cancelBooking(booking: Booking): void {
    if (!this.currentUserId) return;

    if (confirm(`Are you sure you want to cancel your booking for ${booking.groupName}?`)) {
      this.bookingService.cancelBooking(booking.bookingId, this.currentUserId).subscribe({
        next: () => {
          alert('Booking cancelled successfully');
          this.loadBookings();
        },
        error: (error) => {
          alert('Failed to cancel booking');
          console.error(error);
        }
      });
    }
  }

  isUpcoming(booking: Booking): boolean {
    return booking.startTime ? new Date(booking.startTime) > new Date() : false;
  }

  isPast(booking: Booking): boolean {
    return booking.endTime ? new Date(booking.endTime) < new Date() : false;
  }

  getStatusClass(booking: Booking): string {
    if (booking.status === 'Cancelled') return 'status-cancelled';
    if (this.isPast(booking)) return 'status-completed';
    if (this.isUpcoming(booking)) return 'status-upcoming';
    return 'status-confirmed';
  }
}
