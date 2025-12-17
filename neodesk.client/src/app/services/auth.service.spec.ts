import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { AuthResponse, LoginRequest } from '../models/auth.interface';
import { Router } from '@angular/router';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let routerMock: jasmine.SpyObj<Router>;

  beforeEach(() => {
    routerMock = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: Router, useValue: routerMock }
      ]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);

    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('powinien zostać utworzony', () => {
    expect(service).toBeTruthy();
  });

  it('login() powinien wysłać zapytanie POST i zapisać token', () => {
    const mockLoginData: LoginRequest = { email: 'test@test.com', password: 'password' };
    const mockResponse: AuthResponse = {
      token: 'fake-jwt-token',
      user: {
        id: 1,
        email: 'test@test.com',
        name: 'Test',
        role: 'EndUser',
        isActive: true,
        createdAt: '2024-01-01',
        lastLoginAt: '2024-01-02'
      },
      expiresAt: '2024-01-02T12:00:00'
    };

    service.login(mockLoginData).subscribe(response => {
      expect(response).toEqual(mockResponse);
      // ZMIANA: Sprawdzamy poprawny klucz 'neodesk_token'
      expect(localStorage.getItem('neodesk_token')).toBe('fake-jwt-token');
    });

    // ZMIANA: Oczekujemy adresu relatywnego /api/auth/login
    const req = httpMock.expectOne('/api/auth/login');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockLoginData);

    req.flush(mockResponse);
  });

  it('logout() powinien usunąć token z localStorage', () => {
    // ZMIANA: Ustawiamy poprawny klucz przed testem wylogowania
    localStorage.setItem('neodesk_token', 'old-token');

    service.logout();

    // ZMIANA: Sprawdzamy czy 'neodesk_token' zniknął
    expect(localStorage.getItem('neodesk_token')).toBeNull();
    expect(routerMock.navigate).toHaveBeenCalledWith(['/login']);
  });
});
