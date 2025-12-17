import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { AuthService } from '../services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authServiceMock: jasmine.SpyObj<AuthService>;
  let routerMock: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    authServiceMock = jasmine.createSpyObj('AuthService', ['login']);
    routerMock = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [FormsModule, HttpClientTestingModule],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              queryParams: {},
              paramMap: { get: () => null }
            },
            queryParams: of({})
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('powinien zostać utworzony', () => {
    expect(component).toBeTruthy();
  });

  it('powinien wywołać authService.login przy poprawnym formularzu', () => {
    component.loginData = { email: 'test@test.com', password: 'password123' };

    authServiceMock.login.and.returnValue(of({
      token: 'abc',
      user: {
        id: 1,
        name: 'T',
        email: 'e',
        role: 'EndUser',
        isActive: true,
        createdAt: '2024-01-01T10:00:00',
        lastLoginAt: '2024-01-01T12:00:00'
      },
      expiresAt: '2024-01-02T10:00:00'
    }));

    component.onSubmit();

    expect(authServiceMock.login).toHaveBeenCalledWith({ email: 'test@test.com', password: 'password123' });
    expect(routerMock.navigate).toHaveBeenCalledWith(['/']);
  });

  it('powinien wyświetlić błąd przy nieudanym logowaniu', () => {
    component.loginData = { email: 'wrong@test.com', password: 'wrong' };

    const errorResponse = {
      status: 401,
      error: { message: 'Nieprawidłowy email lub hasło' }
    };

    authServiceMock.login.and.returnValue(throwError(() => errorResponse));

    component.onSubmit();

    expect(authServiceMock.login).toHaveBeenCalled();
    expect(component.errorMessage).toBe('Nieprawidłowy email lub hasło');
  });
});
