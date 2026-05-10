import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GroupService } from '../../services/group.service';
import { Group } from '../../models/group.model';

@Component({
  selector: 'app-admin-groups',
  templateUrl: './admin-groups.component.html',
  styleUrl: './admin-groups.component.css',
  standalone: false
})
export class AdminGroupsComponent implements OnInit {
  groups: Group[] = [];
  loading = true;

  constructor(private groupService: GroupService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadGroups();
  }

  loadGroups(): void {
    this.loading = true;
    this.groupService.getAllGroups().subscribe({
      next: (groups) => {
        this.groups = groups;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading groups:', error);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
