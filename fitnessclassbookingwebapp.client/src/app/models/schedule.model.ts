export interface Schedule {
  scheduleId: number;
  groupId: number;
  groupName?: string;
  roomId: number;
  roomName?: string;
  startTime: Date;
  endTime: Date;
  currentBookings?: number;
  maxParticipants?: number;
}

export interface ScheduleDto {
  scheduleId?: number;
  groupId: number;
  groupName?: string;
  roomId: number;
  roomName?: string;
  startTime: Date;
  endTime: Date;
  currentBookings?: number;
  maxParticipants?: number;
}
