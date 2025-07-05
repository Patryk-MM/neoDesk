import { Component, OnInit } from '@angular/core';
import { UserManagementService } from '../services/user-management.service';
import { User, CreateUserRequest, UpdateUserRequest } from '../models/auth.interface';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.scss'
})
export class UserManagementComponent implements OnInit {
  users: User[] = [];
  isLoading = true;
  isCreating = false;
  editingUser: User | null = null;

  showCreateForm = false;
  showEditForm = false;
  showPasswordReset = false;

  newUser: CreateUserRequest = {
    name: '',
    email: '',
    password: '',
    role: 'EndUser'
  };

  editUser: UpdateUserRequest = {
    name: '',
    email: '',
    role: 'EndUser',
    isActive: true
  };

  resetPassword = {
    userId: 0,
    password: ''
  };

  roles = [
    { value: 'EndUser', label: 'Użytkownik końcowy' },
    { value: 'Technician', label: 'Technik' },
    { value: 'Admin', label: 'Administrator' }
  ];

  constructor(private userManagementService: UserManagementService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.userManagementService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading users:', error);
        this.isLoading = false;
      }
    });
  }

  openCreateForm(): void {
    this.resetNewUserForm();
    this.showCreateForm = true;
  }

  closeCreateForm(): void {
    this.showCreateForm = false;
    this.resetNewUserForm();
  }

  createUser(): void {
    if (!this.newUser.name || !this.newUser.email || !this.newUser.password) {
      alert('Proszę wypełnić wszystkie pola');
      return;
    }

    this.isCreating = true;
    this.userManagementService.createUser(this.newUser).subscribe({
      next: (user) => {
        alert('Użytkownik został utworzony pomyślnie');
        this.loadUsers();
        this.closeCreateForm();
        this.isCreating = false;
      },
      error: (error) => {
        console.error('Error creating user:', error);
        alert(error.error?.message || 'Wystąpił błąd podczas tworzenia użytkownika');
        this.isCreating = false;
      }
    });
  }

  openEditForm(user: User): void {
    this.editingUser = user;
    this.editUser = {
      name: user.name,
      email: user.email,
      role: user.role,
      isActive: user.isActive
    };
    this.showEditForm = true;
  }

  closeEditForm(): void {
    this.showEditForm = false;
    this.editingUser = null;
  }

  updateUser(): void {
    if (!this.editingUser || !this.editUser.name || !this.editUser.email) {
      alert('Proszę wypełnić wszystkie pola');
      return;
    }

    this.userManagementService.updateUser(this.editingUser.id, this.editUser).subscribe({
      next: (user) => {
        alert('Użytkownik został zaktualizowany pomyślnie');
        this.loadUsers();
        this.closeEditForm();
      },
      error: (error) => {
        console.error('Error updating user:', error);
        alert(error.error?.message || 'Wystąpił błąd podczas aktualizacji użytkownika');
      }
    });
  }

  openPasswordReset(user: User): void {
    this.resetPassword.userId = user.id;
    this.resetPassword.password = '';
    this.showPasswordReset = true;
  }

  closePasswordReset(): void {
    this.showPasswordReset = false;
    this.resetPassword = { userId: 0, password: '' };
  }

  resetUserPassword(): void {
    if (!this.resetPassword.password || this.resetPassword.password.length < 6) {
      alert('Hasło musi mieć co najmniej 6 znaków');
      return;
    }

    this.userManagementService.resetPassword(this.resetPassword.userId, this.resetPassword.password).subscribe({
      next: () => {
        alert('Hasło zostało zresetowane pomyślnie');
        this.closePasswordReset();
      },
      error: (error) => {
        console.error('Error resetting password:', error);
        alert('Wystąpił błąd podczas resetowania hasła');
      }
    });
  }

  toggleUserStatus(user: User): void {
    const action = user.isActive ? 'dezaktywować' : 'aktywować';
    if (!confirm(`Czy na pewno chcesz ${action} użytkownika ${user.name}?`)) {
      return;
    }

    this.userManagementService.toggleUserStatus(user.id).subscribe({
      next: (updatedUser) => {
        const index = this.users.findIndex(u => u.id === user.id);
        if (index !== -1) {
          this.users[index] = updatedUser;
        }
        alert(`Użytkownik został ${user.isActive ? 'dezaktywowany' : 'aktywowany'} pomyślnie`);
      },
      error: (error) => {
        console.error('Error toggling user status:', error);
        alert('Wystąpił błąd podczas zmiany statusu użytkownika');
      }
    });
  }

  deleteUser(user: User): void {
    if (!confirm(`Czy na pewno chcesz usunąć użytkownika ${user.name}? Ta operacja jest nieodwracalna.`)) {
      return;
    }

    this.userManagementService.deleteUser(user.id).subscribe({
      next: () => {
        alert('Użytkownik został usunięty pomyślnie');
        this.loadUsers();
      },
      error: (error) => {
        console.error('Error deleting user:', error);
        alert(error.error?.message || 'Wystąpił błąd podczas usuwania użytkownika');
      }
    });
  }

  getRoleLabel(role: string): string {
    const roleObj = this.roles.find(r => r.value === role);
    return roleObj ? roleObj.label : role;
  }

  private resetNewUserForm(): void {
    this.newUser = {
      name: '',
      email: '',
      password: '',
      role: 'EndUser'
    };
  }
}
