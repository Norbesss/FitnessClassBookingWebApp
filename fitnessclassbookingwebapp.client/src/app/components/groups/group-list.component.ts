import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GroupService } from '../../services/group.service';
import { Group } from '../../models/group.model';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrl: './group-list.component.css',
  standalone: false
})
export class GroupListComponent implements OnInit {
  groups: Group[] = [];
  filteredGroups: Group[] = [];
  searchTerm = '';
  loading = true;

  constructor(
    private groupService: GroupService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadGroups();
  }

  loadGroups(): void {
    this.groupService.getAllGroups().subscribe({
      next: (groups) => {
        this.groups = groups;
        this.filteredGroups = groups;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading groups:', error);
        this.loading = false;
      }
    });
  }

  filterGroups(): void {
    if (!this.searchTerm) {
      this.filteredGroups = this.groups;
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredGroups = this.groups.filter(group =>
        group.name.toLowerCase().includes(term) ||
        group.description.toLowerCase().includes(term) ||
        group.coachName?.toLowerCase().includes(term)
      );
    }
  }

  viewDetails(id: number): void {
    this.router.navigate(['/groups', id]);
  }

  getRatingStars(rating: number): string[] {
    const stars: string[] = [];
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;

    for (let i = 0; i < fullStars; i++) {
      stars.push('filled');
    }
    if (hasHalfStar) {
      stars.push('half');
    }
    while (stars.length < 5) {
      stars.push('empty');
    }
    return stars;
  }
}
