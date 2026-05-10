import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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
  isAuthenticated = false;
  currentUserId: number | null = null;
  groupId!: number;
  userReview: Review | null = null;
  isLoaded = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private groupService: GroupService,
    private scheduleService: ScheduleService,
    private reviewService: ReviewService,
    private bookingService: BookingService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
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

    this.groupId = Number(this.route.snapshot.paramMap.get('id'));

    this.loadGroupDetails(this.groupId);
    this.loadUserReview();
  }

  loadGroupDetails(id: number): void {
    this.isLoaded = false;
    this.groupService.getGroupById(id).subscribe({
      next: (group) => {
        this.group = group;
        this.loadSchedules(id).subscribe({
          next: (schedules) => {
            this.schedules = schedules.filter(s => new Date(s.startTime) > new Date());
            this.loadReviews(id).subscribe({
              next: (reviews) => {
                this.reviews = reviews;
                this.isLoaded = true;
                this.cdr.detectChanges();
              },
              error: (error) => {
                console.error('Error loading reviews:', error);
                this.isLoaded = true;
                this.cdr.detectChanges();
              }
            });
          },
          error: (error) => {
            console.error('Error loading schedules:', error);
            this.isLoaded = true;
            this.cdr.detectChanges();
          }
        });
      },
      error: (error) => {
        console.error('Error loading group:', error);
        this.isLoaded = true;
        this.cdr.detectChanges();
      }
    });
  }

  loadSchedules(groupId: number) {
    return this.scheduleService.getSchedulesByGroup(groupId);
  }

  loadReviews(groupId: number) {
    return this.reviewService.getReviewsByGroup(groupId);
  }

  loadUserReview(): void {
    this.reviewService.getMyReviewForGroup(this.groupId).subscribe({
      next: (review) => {
        this.userReview = review;
      },
      error: () => {
        this.userReview = null;
      }
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
        this.cdr.detectChanges();
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
