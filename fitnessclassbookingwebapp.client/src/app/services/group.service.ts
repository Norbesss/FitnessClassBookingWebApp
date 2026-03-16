import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Group, GroupDto } from '../models/group.model';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private readonly API_URL = '/api/groups';

  constructor(private http: HttpClient) {}

  getAllGroups(): Observable<Group[]> {
    return this.http.get<Group[]>(this.API_URL);
  }

  getGroupById(id: number): Observable<Group> {
    return this.http.get<Group>(`${this.API_URL}/${id}`);
  }

  createGroup(groupDto: GroupDto): Observable<Group> {
    return this.http.post<Group>(this.API_URL, groupDto);
  }

  updateGroup(id: number, groupDto: GroupDto): Observable<Group> {
    return this.http.put<Group>(`${this.API_URL}/${id}`, groupDto);
  }

  deleteGroup(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
