import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReviewService } from '../../../services/review.service';
import { Review } from '../../../models/review.model';

@Component({
  selector: 'app-group-review-update',
  templateUrl: './group-review-update.component.html',
  styleUrls: ['./group-review-update.component.css'],
  standalone: false
})
export class GroupReviewUpdateComponent implements OnInit {
  groupId!: number;
  review: Review | null = null;

  rating = 5;
  comment = '';

  loading = true;
  saving = false;
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
    this.loadMyReview();
  }

  loadMyReview(): void {
    this.loading = true;

    this.reviewService.getMyReviewForGroup(this.groupId).subscribe({
      next: (review) => {
        this.review = review;
        this.rating = review.rating;
        this.comment = review.comment;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);

        if (error.status === 404) {
          this.errorMessage = 'You have not written a review for this class yet.';
        } else {
          this.errorMessage = 'Failed to load review.';
        }

        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  updateReview(): void {
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

    this.saving = true;

    this.reviewService.updateMyReviewForGroup(this.groupId, {
      rating: this.rating,
      comment: this.comment
    }).subscribe({
      next: () => {
        this.successMessage = 'Review updated successfully.';
        this.saving = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);

        if (error.status === 404) {
          this.errorMessage = 'You can only update an existing review.';
        } else if (error.status === 403) {
          this.errorMessage = 'You are not allowed to update this review.';
        } else {
          this.errorMessage = 'Failed to update review.';
        }

        this.saving = false;
        this.cdr.detectChanges();
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/groups', this.groupId]);
  }
}
