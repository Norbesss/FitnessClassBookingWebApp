export interface Review {
  reviewId: number;
  userId: number;
  userName?: string;
  groupId: number;
  groupName?: string;
  rating: number;
  comment: string;
  createdAt: Date;
}

export interface ReviewDto {
  reviewId?: number;
  userId: number;
  userName?: string;
  groupId: number;
  groupName?: string;
  rating: number;
  comment: string;
  createdAt?: Date;
}
