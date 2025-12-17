import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TicketService } from '../services/ticket.service';
import { Ticket, CreateTicket } from '../models/ticket.interface';
import { TicketStatus, statusOptions } from '../models/ticket.enums'

@Component({
  selector: 'app-ticket-detail',
  templateUrl: './ticket-detail.component.html',
  styleUrl: './ticket-detail.component.scss'
})
export class TicketDetailComponent implements OnInit {
  ticket: Ticket | null = null;
  editTicket: CreateTicket = {
    title: '',
    description: '',
    category: 0,
    status: 0,
    createdByUserId: 1,
  };

  isLoading = true;
  isEditing = false;
  isSaving = false;
  isDeleting = false;
  ticketId: number = 0;


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ticketService: TicketService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.ticketId = +params['id'];
      this.loadTicket();
    });
  }

  loadTicket(): void {
    this.isLoading = true;
    this.ticketService.getTicket(this.ticketId).subscribe({
      next: (ticket) => {
        this.ticket = ticket;
        this.populateEditForm();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading ticket:', error);
        this.isLoading = false;
        alert('Nie można załadować zgłoszenia');
        this.router.navigate(['/']);
      }
    });
  }

  populateEditForm(): void {
    if (this.ticket) {
      this.editTicket = {
        title: this.ticket.title,
        description: this.ticket.description,
        category: this.ticket.category,
        status: this.ticket.status,
        createdByUserId: 1, // Will need to be updated when user system is implemented
        createdAt: new Date(this.ticket.createdAt).toISOString().slice(0, 16)
      };
    }
  }

  // getCategoryValue(categoryName: string): number {
  //   switch (categoryName.toLowerCase()) {
  //     case 'software':
  //     case 'oprogramowanie':
  //       return 0;
  //     case 'hardware':
  //     case 'sprzęt':
  //       return 1;
  //     default:
  //       return 0;
  //   }
  // }

  getStatusValue(statusName: string): number {
    switch (statusName.toLowerCase()) {
      case 'new':
      case 'nowy':
        return 0;
      case 'assigned':
      case 'przypisany':
        return 1;
      case 'suspended':
      case 'wstrzymany':
        return 2;
      case 'solved':
      case 'rozwiązany':
        return 3;
      default:
        return 0;
    }
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      // Reset form when canceling edit
      this.populateEditForm();
    }
  }

  saveTicket(): void {
    if (!this.editTicket.title.trim() || !this.editTicket.description.trim()) {
      alert('Proszę wypełnić wszystkie wymagane pola');
      return;
    }

    this.isSaving = true;
    this.ticketService.updateTicket(this.ticketId, this.editTicket).subscribe({
      next: () => {
        alert('Zgłoszenie zostało zaktualizowane pomyślnie!');
        this.isEditing = false;
        this.isSaving = false;
        this.loadTicket(); // Reload to show updated data
      },
      error: (error) => {
        console.error('Error updating ticket:', error);
        alert('Wystąpił błąd podczas aktualizacji zgłoszenia');
        this.isSaving = false;
      }
    });
  }

  deleteTicket(): void {
    if (!confirm('Czy na pewno chcesz usunąć to zgłoszenie? Ta operacja jest nieodwracalna.')) {
      return;
    }

    this.isDeleting = true;
    this.ticketService.deleteTicket(this.ticketId).subscribe({
      next: () => {
        alert('Zgłoszenie zostało usunięte pomyślnie!');
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.error('Error deleting ticket:', error);
        alert('Wystąpił błąd podczas usuwania zgłoszenia');
        this.isDeleting = false;
      }
    });
  }

  onDescriptionChange(content: any): void {
    this.editTicket.description = content.html || content.text || '';
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  protected readonly statusOptions = statusOptions;
}
