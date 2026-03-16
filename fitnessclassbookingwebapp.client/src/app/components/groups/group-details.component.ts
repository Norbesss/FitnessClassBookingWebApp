import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GroupService } from '../../services/group.service';
import { ScheduleService } from '../../services/schedule.service';
import { ReviewService } from '../../services/review.service';
import { BookingService } from '../../services/booking.service';
import { AuthService } from '../../services/auth.service';
import { Group } from '../../models/group.model';
import { Schedule } from '../../models/schedule.model';
import { Review } from '../../models/review.model';

@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html',
  styleUrl: './group-details.component.css',
  standalone: false
})
export class GroupDetailsComponent implements OnInit {
  group: Group | null = null;
  schedules: Schedule[] = [];
  reviews: Review[] = [];
  loading = true;
  isAuthenticated = false;
  currentUserId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private groupService: GroupService,
    private scheduleService: ScheduleService,
    private reviewService: ReviewService,
    private bookingService: BookingService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.isAuthenticated = user !== null;
      this.currentUserId = user?.userId || null;
    });

    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadGroupDetails(id);
    }
  }

  loadGroupDetails(id: number): void {
    this.groupService.getGroupById(id).subscribe({
      next: (group) => {
        this.group = group;
        this.loadSchedules(id);
        this.loadReviews(id);
      },
      error: (error) => {
        console.error('Error loading group:', error);
        this.loading = false;
      }
    });
  }

  loadSchedules(groupId: number): void {
    this.scheduleService.getSchedulesByGroup(groupId).subscribe({
      next: (schedules) => {
        this.schedules = schedules.filter(s => new Date(s.startTime) > new Date());
        this.loading = false;
      },
      error: (error) => console.error('Error loading schedules:', error)
    });
  }

  loadReviews(groupId: number): void {
    this.reviewService.getReviewsByGroup(groupId).subscribe({
      next: (reviews) => {
        this.reviews = reviews;
      },
      error: (error) => console.error('Error loading reviews:', error)
    });
  }

  bookClass(scheduleId: number): void {
    if (!this.isAuthenticated || !this.currentUserId) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.url } });
      return;
    }

    this.bookingService.createBooking({
      userId: this.currentUserId,
      scheduleId: scheduleId
    }).subscribe({
      next: () => {
        alert('Class booked successfully!');
        this.loadSchedules(this.group!.groupId);
      },
      error: (error) => {
        alert(error.error?.message || 'Failed to book class');
      }
    });
  }

  getAverageRating(): number {
    if (this.reviews.length === 0) return 0;
    const sum = this.reviews.reduce((acc, review) => acc + review.rating, 0);
    return sum / this.reviews.length;
  }

  getStars(rating: number): string {
    return '★'.repeat(Math.floor(rating)) + '☆'.repeat(5 - Math.floor(rating));
  }
}
