export interface LoginRequest {
  email: string;
  password: string;
}

export interface CreateUserRequest {
  name: string;
  email: string;
  password: string;
  role: string; // 'EndUser', 'Technician', 'Admin'
}

export interface UpdateUserRequest {
  name: string;
  email: string;
  role: string;
  isActive: boolean;
  password?: string; // Optional - only if changing password
}

export interface AuthResponse {
  token: string;
  user: User;
  expiresAt: string;
}

export interface User {
  id: number;
  name: string;
  email: string;
  role: string;
  isActive: boolean;
  createdAt: string;
  lastLoginAt?: string;
}
