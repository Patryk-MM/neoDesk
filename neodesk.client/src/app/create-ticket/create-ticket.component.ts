import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TicketService } from '../services/ticket.service';
import { CreateTicket } from '../models/ticket.interface';

@Component({
  selector: 'app-create-ticket',
  templateUrl: './create-ticket.component.html',
  styleUrl: './create-ticket.component.scss'
})
export class CreateTicketComponent {
  ticket: CreateTicket = {
    title: '',
    description: '',
    category: 0, // Software
    status: 0,   // New
    createdByUserId: 1, // Default user for now
    createdAt: ''
  };

  isSubmitting = false;

  constructor(
    private ticketService: TicketService,
    private router: Router
  ) {
    // Set current date/time as default
    this.ticket.createdAt = new Date().toISOString().slice(0, 16);
  }

  onSubmit(event: Event): void {
    event.preventDefault();

    if (!this.ticket.title.trim() || !this.ticket.description.trim()) {
      alert('Proszę wypełnić wszystkie wymagane pola');
      return;
    }

    this.isSubmitting = true;

    this.ticketService.createTicket(this.ticket).subscribe({
      next: (createdTicket) => {
        console.log('Ticket created:', createdTicket);
        alert('Zgłoszenie zostało utworzone pomyślnie!');
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.error('Error creating ticket:', error);
        alert('Wystąpił błąd podczas tworzenia zgłoszenia');
        this.isSubmitting = false;
      }
    });
  }

  onDescriptionChange(content: any): void {
    this.ticket.description = content.html || content.text || '';
  }
}
