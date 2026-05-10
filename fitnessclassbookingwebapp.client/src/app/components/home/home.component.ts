import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HomeService, HomePageData, DashboardStats } from '../../services/home.service';

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
  error: string | null = null;
  isLoaded = false;

  constructor(
    public authService: AuthService,
    private homeService: HomeService,
    public router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      this.isAuthenticated = user !== null;
    });

    this.loadHomePageData();
  }

  loadHomePageData(): void {
    this.error = null;
    this.isLoaded = false;

    this.homeService.getHomePageData().subscribe({
      next: (data: HomePageData) => {
        this.stats = data.stats;
        this.featuredGroups = data.featuredGroups;
        this.upcomingSchedules = data.upcomingSchedules;
        this.recentReviews = data.recentReviews;
        this.isLoaded = true;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading home page data:', error);
        this.error = 'Failed to load home page data. Please try again later.';
        this.isLoaded = true;
        this.cdr.detectChanges();
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
