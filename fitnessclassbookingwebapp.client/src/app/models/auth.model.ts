export interface User {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  createdAt: Date;
  isActive: boolean;
  roles?: string[];
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber?: string;
}

export interface AuthResponseDto {
  userId: number;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[];
  token: string;
}
