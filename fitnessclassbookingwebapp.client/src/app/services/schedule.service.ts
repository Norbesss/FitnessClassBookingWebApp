import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Schedule, ScheduleDto } from '../models/schedule.model';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {
  private readonly API_URL = '/api/schedules';

  constructor(private http: HttpClient) {}

  getAllSchedules(): Observable<Schedule[]> {
    return this.http.get<Schedule[]>(this.API_URL);
  }

  getScheduleById(id: number): Observable<Schedule> {
    return this.http.get<Schedule>(`${this.API_URL}/${id}`);
  }

  getSchedulesByGroup(groupId: number): Observable<Schedule[]> {
    return this.http.get<Schedule[]>(`${this.API_URL}/group/${groupId}`);
  }

  getUpcomingSchedules(): Observable<Schedule[]> {
    return this.http.get<Schedule[]>(`${this.API_URL}/upcoming`);
  }

  createSchedule(scheduleDto: ScheduleDto): Observable<Schedule> {
    return this.http.post<Schedule>(this.API_URL, scheduleDto);
  }

  updateSchedule(id: number, scheduleDto: ScheduleDto): Observable<Schedule> {
    return this.http.put<Schedule>(`${this.API_URL}/${id}`, scheduleDto);
  }

  deleteSchedule(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
