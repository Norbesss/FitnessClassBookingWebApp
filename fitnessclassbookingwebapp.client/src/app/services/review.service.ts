import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Review, ReviewDto } from '../models/review.model';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private readonly API_URL = '/api/reviews';

  constructor(private http: HttpClient) {}

  getAllReviews(): Observable<Review[]> {
    return this.http.get<Review[]>(this.API_URL);
  }

  getReviewById(id: number): Observable<Review> {
    return this.http.get<Review>(`${this.API_URL}/${id}`);
  }

  getReviewsByGroup(groupId: number): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.API_URL}/group/${groupId}`);
  }

  getReviewsByUser(userId: number): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.API_URL}/user/${userId}`);
  }

  createReview(reviewDto: ReviewDto): Observable<Review> {
    return this.http.post<Review>(this.API_URL, reviewDto);
  }

  updateReview(id: number, reviewDto: ReviewDto): Observable<Review> {
    return this.http.put<Review>(`${this.API_URL}/${id}`, reviewDto);
  }

  deleteReview(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
