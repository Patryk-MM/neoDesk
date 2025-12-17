import { Component } from '@angular/core';
import { TicketStatus, TicketCategory } from '../models/ticket.enums'

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.scss'
})
export class TicketListComponent {
  eStatus = TicketStatus;
  eCategory = TicketCategory;

  statusLabels: Record<number, string> = {
    [TicketStatus.New]: 'Nowy',
    [TicketStatus.Assigned]: 'Przypisany',
    [TicketStatus.Suspended]: 'Zawieszony',
    [TicketStatus.Solved]: 'Rozwiązany'
  }

  categoryLabels: Record<number, string> = {
    [TicketCategory.Software]: 'Oprogramowanie',
    [TicketCategory.Hardware]: 'Sprzęt'
  }
}
