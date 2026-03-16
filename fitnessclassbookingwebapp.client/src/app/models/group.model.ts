export interface Group {
  groupId: number;
  name: string;
  description: string;
  coachId: number;
  coachName?: string;
  maxParticipants: number;
  averageRating?: number;
  totalReviews?: number;
}

export interface GroupDto {
  groupId?: number;
  name: string;
  description: string;
  coachId: number;
  coachName?: string;
  maxParticipants: number;
  averageRating?: number;
  totalReviews?: number;
}
