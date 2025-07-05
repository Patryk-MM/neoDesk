import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { LoginRequest, AuthResponse, User } from '../models/auth.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.baseUrl}api/auth`;
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);

  public currentUser$ = this.currentUserSubject.asObservable();
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    // Check if user is already logged in on service initialization
    this.initializeAuth();
  }

  private initializeAuth(): void {
    const token = this.getToken();
    const userData = localStorage.getItem('neodesk_user');

    if (token && userData) {
      try {
        const user = JSON.parse(userData);
        this.currentUserSubject.next(user);
        this.isLoggedInSubject.next(true);
      } catch (error) {
        console.error('Error parsing stored user data:', error);
        this.logout();
      }
    }
  }

  login(loginData: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, loginData)
      .pipe(
        tap(response => {
          this.setSession(response);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('neodesk_token');
    localStorage.removeItem('neodesk_user');
    localStorage.removeItem('neodesk_expires_at');

    this.currentUserSubject.next(null);
    this.isLoggedInSubject.next(false);

    this.router.navigate(['/login']);
  }

  getCurrentUser(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/me`);
  }

  getToken(): string | null {
    return localStorage.getItem('neodesk_token');
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    const expiresAt = localStorage.getItem('neodesk_expires_at');

    if (!token || !expiresAt) {
      return false;
    }

    return new Date().getTime() < parseInt(expiresAt);
  }

  getCurrentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  hasRole(role: string): boolean {
    const user = this.getCurrentUserValue();
    return user?.role === role;
  }

  hasAnyRole(roles: string[]): boolean {
    const user = this.getCurrentUserValue();
    return user ? roles.includes(user.role) : false;
  }

  private setSession(authResponse: AuthResponse): void {
    const expiresAt = new Date(authResponse.expiresAt).getTime();

    localStorage.setItem('neodesk_token', authResponse.token);
    localStorage.setItem('neodesk_user', JSON.stringify(authResponse.user));
    localStorage.setItem('neodesk_expires_at', expiresAt.toString());

    this.currentUserSubject.next(authResponse.user);
    this.isLoggedInSubject.next(true);
  }
}
