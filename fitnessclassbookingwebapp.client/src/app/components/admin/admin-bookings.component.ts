import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { BookingService } from '../../services/booking.service';
import { Booking } from '../../models/booking.model';

@Component({
  selector: 'app-admin-bookings',
  templateUrl: './admin-bookings.component.html',
  styleUrl: './admin-bookings.component.css',
  standalone: false
})
export class AdminBookingsComponent implements OnInit {
  bookings: Booking[] = [];
  loading = true;
  errorMessage = '';

  constructor(private bookingService: BookingService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.loading = true;
    this.errorMessage = '';

    this.bookingService.getAllBookings().subscribe({
      next: (bookings) => {
        this.bookings = bookings;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading bookings:', error);
        this.errorMessage = 'Failed to load bookings.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  confirmBooking(bookingId: number): void {
    this.bookingService.updateBookingStatus(bookingId, 'Confirmed').subscribe({
      next: () => {
        this.loadBookings();
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error confirming booking:', error);
        this.errorMessage = 'Failed to confirm booking.';
        this.cdr.detectChanges();
      }
    });
  }

  cancelBooking(bookingId: number): void {
    this.bookingService.updateBookingStatus(bookingId, 'Cancelled').subscribe({
      next: () => {
        this.loadBookings();
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error cancelling booking:', error);
        this.errorMessage = 'Failed to cancel booking.';
        this.cdr.detectChanges();
      }
    });
  }
}
