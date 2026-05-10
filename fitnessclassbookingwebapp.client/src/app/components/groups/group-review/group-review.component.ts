import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReviewService } from '../../../services/review.service';
import { Review } from '../../../models/review.model';

@Component({
  selector: 'app-group-review',
  templateUrl: './group-review.component.html',
  styleUrls: ['./group-review.component.css'],
  standalone: false
})
export class GroupReviewComponent implements OnInit {
  groupId!: number;
  reviews: Review[] = [];

  rating = 5;
  comment = '';

  loading = true;
  submitting = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private reviewService: ReviewService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.groupId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadReviews();
  }

  loadReviews(): void {
    this.loading = true;

    this.reviewService.getReviewsByGroup(this.groupId).subscribe({
      next: (reviews) => {
        this.reviews = reviews;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading reviews:', error);
        this.errorMessage = 'Failed to load reviews.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  submitReview(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.rating < 1 || this.rating > 5) {
      this.errorMessage = 'Rating must be between 1 and 5.';
      return;
    }

    if (!this.comment.trim()) {
      this.errorMessage = 'Comment is required.';
      return;
    }

    this.submitting = true;

    this.reviewService.createReviewForGroup(this.groupId, {
      rating: this.rating,
      comment: this.comment
    }).subscribe({
      next: () => {
        this.comment = '';
        this.rating = 5;
        this.submitting = false;
        this.loadReviews();
      },
      error: (error) => {
        console.error('Error creating review:', error);

        if (error.status === 403) {
          this.errorMessage = 'You can only review classes you are assigned to.';
        } else if (error.status === 409) {
          this.errorMessage = 'You have already reviewed this class.';
        } else {
          this.errorMessage = 'Failed to create review.';
        }

        this.submitting = false;
        this.cdr.detectChanges();
      }
    });
  }

  deleteReview(reviewId: number): void {
    const confirmed = confirm('Are you sure you want to delete this review?');

    if (!confirmed) {
      return;
    }

    this.reviewService.deleteReview(reviewId).subscribe({
      next: () => this.loadReviews(),
      error: (error) => {
        console.error('Error deleting review:', error);

        if (error.status === 403) {
          this.errorMessage = 'You can only delete your own review.';
        } else {
          this.errorMessage = 'Failed to delete review.';
        }
        this.cdr.detectChanges();
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/groups', this.groupId]);
  }
}
