import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { Statistics } from '../../models/admin.model';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css',
  standalone: false
})
export class AdminDashboardComponent implements OnInit {
  statistics: Statistics | null = null;
  isLoaded = false;

  constructor(
    private adminService: AdminService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadStatistics();
  }

  loadStatistics(): void {
    this.isLoaded = false;
    this.adminService.getStatistics().subscribe({
      next: (stats) => {
        this.statistics = stats;
        this.isLoaded = true;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading statistics:', error);
        this.isLoaded = true;
        this.cdr.detectChanges();
      }
    });
  }
}
