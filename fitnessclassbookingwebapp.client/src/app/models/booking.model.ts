export interface Booking {
  bookingId: number;
  userId: number;
  userName?: string;
  scheduleId: number;
  groupName?: string;
  startTime?: Date;
  endTime?: Date;
  status: string;
  createdAt: Date;
}

export interface BookingDto {
  bookingId?: number;
  userId: number;
  userName?: string;
  scheduleId: number;
  groupName?: string;
  startTime?: Date;
  endTime?: Date;
  status: string;
  createdAt?: Date;
}

export interface CreateBookingRequest {
  userId: number;
  scheduleId: number;
}
