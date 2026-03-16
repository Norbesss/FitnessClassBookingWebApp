import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DashboardStats {
  totalGroups: number;
  totalSchedules: number;
  upcomingSchedules: number;
  totalActiveUsers: number;
  totalBookings: number;
  averageRating: number;
}

export interface HomePageData {
  stats: DashboardStats;
  featuredGroups: any[];
  upcomingSchedules: any[];
  recentReviews: any[];
}

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private apiUrl = '/api/home';

  constructor(private http: HttpClient) {}

  getHomePageData(): Observable<HomePageData> {
    return this.http.get<HomePageData>(this.apiUrl);
  }

  getDashboardStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.apiUrl}/stats`);
  }

  getFeaturedGroups(count: number = 3): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/featured-groups?count=${count}`);
  }

  getUpcomingSchedules(count: number = 5): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/upcoming-schedules?count=${count}`);
  }

  getRecentReviews(count: number = 5): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/recent-reviews?count=${count}`);
  }
}
