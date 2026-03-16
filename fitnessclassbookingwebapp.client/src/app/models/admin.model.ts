export interface Room {
  roomId: number;
  name: string;
  capacity: number;
}

export interface Role {
  roleId: number;
  name: string;
}

export interface Statistics {
  totalUsers: number;
  activeUsers: number;
  totalGroups: number;
  totalSchedules: number;
  upcomingSchedules: number;
  totalBookings: number;
  confirmedBookings: number;
  totalReviews: number;
  averageRating: number;
}
