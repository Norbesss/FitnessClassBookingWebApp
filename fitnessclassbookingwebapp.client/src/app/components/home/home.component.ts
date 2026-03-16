import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HomeService, HomePageData, DashboardStats } from '../../services/home.service';
import { Group } from '../../models/group.model';
import { Schedule } from '../../models/schedule.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  standalone: false
})
export class HomeComponent implements OnInit {
  isAuthenticated = false;
  currentUser: any = null;
  featuredGroups: any[] = [];
  upcomingSchedules: any[] = [];
  recentReviews: any[] = [];
  stats: DashboardStats | null = null;
  loading = true;
  error: string | null = null;

  constructor(
    public authService: AuthService,
    private homeService: HomeService,
    public router: Router
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      this.isAuthenticated = user !== null;
    });

    this.loadHomePageData();
  }

  loadHomePageData(): void {
    this.loading = true;
    this.error = null;

    this.homeService.getHomePageData().subscribe({
      next: (data: HomePageData) => {
        this.stats = data.stats;
        this.featuredGroups = data.featuredGroups;
        this.upcomingSchedules = data.upcomingSchedules;
        this.recentReviews = data.recentReviews;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading home page data:', error);
        this.error = 'Failed to load home page data. Please try again later.';
        this.loading = false;
      }
    });
  }

  viewAllGroups(): void {
    this.router.navigate(['/groups']);
  }

  viewGroupDetails(groupId: number): void {
    this.router.navigate(['/groups', groupId]);
  }

  getRatingStars(rating: number): string[] {
    const stars: string[] = [];
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;

    for (let i = 0; i < fullStars; i++) {
      stars.push('star');
    }
    if (hasHalfStar) {
      stars.push('star_half');
    }
    while (stars.length < 5) {
      stars.push('star_border');
    }
    return stars;
  }
}
