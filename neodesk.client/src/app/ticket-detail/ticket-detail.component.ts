import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {TicketService} from '../services/ticket.service';
import {AssignTicket, Ticket, UpdateTicket} from '../models/ticket.interface';
import {
  statusOptions,
  categoryOptions,
  getStatusLabel,
  getCategoryLabel,
  TicketCategory,
  TicketStatus
} from '../models/ticket.enums'
import {SimpleUserDTO} from "../models/user.interface";
import {LookupService} from "../services/lookup.service";
import {STATUS_CLASSES} from "../models/ticket.constants";

@Component({
  selector: 'app-ticket-detail',
  templateUrl: './ticket-detail.component.html',
  styleUrl: './ticket-detail.component.scss'
})
export class TicketDetailComponent implements OnInit {
  ticket: Ticket | null = null;
  editTicket: UpdateTicket = {
    title: '',
    description: '',
    category: TicketCategory.Software,
    status: TicketStatus.Solved,
  };
  assignTicket: AssignTicket = {
    assignedToUserId: this.ticket?.assignedTo?.id,
  }

  isLoading = true;
  isEditing = false;
  isSaving = false;
  isDeleting = false;
  ticketId: number = 0;
  technicians: SimpleUserDTO[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ticketService: TicketService,
    private lookupService: LookupService,
  ) {
  }

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
        status: this.ticket.status
      };
      this.lookupService.getTechnicians().subscribe({
        next: (data) => {
          this.technicians = data;
        },
        error: (error) => {
          console.error('Błąd pobierania listy techników: ', error);
        }
      })
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

  protected onTechnicianChange(newUserId: number | null): void {
    if (!this.ticket) return;

    this.isLoading = true;



    this.ticketService.assignTicket(this.ticket.id, newUserId).subscribe({
      next: () => {
        this.loadTicket();
        console.log('Technik zmieniony.');
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Błąd w trakcie zmiany technika.', error);
        this.isLoading = false;

      }
    })
  }

  protected readonly statusOptions = statusOptions;
  protected readonly categoryOptions = categoryOptions;
  protected readonly getStatusLabel = getStatusLabel;
  protected readonly getCategoryLabel = getCategoryLabel;

  protected readonly STATUS_CLASSES = STATUS_CLASSES;
}
