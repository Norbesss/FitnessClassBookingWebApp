import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ScheduleService } from '../../services/schedule.service';
import { Schedule } from '../../models/schedule.model';

@Component({
  selector: 'app-admin-schedules',
  templateUrl: './admin-schedules.component.html',
  styleUrl: './admin-schedules.component.css',
  standalone: false
})
export class AdminSchedulesComponent implements OnInit {
  schedules: Schedule[] = [];
  loading = true;

  constructor(private scheduleService: ScheduleService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadSchedules();
  }

  loadSchedules(): void {
    this.loading = true;
    this.scheduleService.getAllSchedules().subscribe({
      next: (schedules) => {
        this.schedules = schedules;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading schedules:', error);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
