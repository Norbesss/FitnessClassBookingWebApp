import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/auth.model';
import { Room, Role, Statistics } from '../models/admin.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private readonly API_URL = '/api/admin';

  constructor(private http: HttpClient) {}

  // User Management
  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.API_URL}/users`);
  }

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.API_URL}/users/${id}`);
  }

  toggleUserActive(id: number): Observable<{ userId: number; isActive: boolean }> {
    return this.http.patch<{ userId: number; isActive: boolean }>(
      `${this.API_URL}/users/${id}/toggle-active`,
      {}
    );
  }

  // Role Management
  getAllRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.API_URL}/roles`);
  }

  assignRoleToUser(userId: number, roleId: number): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.API_URL}/users/${userId}/roles/${roleId}`,
      {}
    );
  }

  removeRoleFromUser(userId: number, roleId: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/users/${userId}/roles/${roleId}`);
  }

  // Room Management
  getAllRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(`${this.API_URL}/rooms`);
  }

  getRoomById(id: number): Observable<Room> {
    return this.http.get<Room>(`${this.API_URL}/rooms/${id}`);
  }

  createRoom(room: Room): Observable<Room> {
    return this.http.post<Room>(`${this.API_URL}/rooms`, room);
  }

  updateRoom(id: number, room: Room): Observable<Room> {
    return this.http.put<Room>(`${this.API_URL}/rooms/${id}`, room);
  }

  deleteRoom(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/rooms/${id}`);
  }

  // Statistics
  getStatistics(): Observable<Statistics> {
    return this.http.get<Statistics>(`${this.API_URL}/statistics`);
  }
}
