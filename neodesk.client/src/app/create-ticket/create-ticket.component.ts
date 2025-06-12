import { Component } from '@angular/core';

@Component({
  selector: 'app-create-ticket',
  templateUrl: './create-ticket.component.html',
  styleUrl: './create-ticket.component.scss'
})
export class CreateTicketComponent {
  statusOptions = [
    { label: 'Nowy', value: 'nowy', class: 'status-primary' },
    { label: 'W toku', value: 'w-toku', class: 'status-warning' },
    { label: 'Zamknięty', value: 'zamkniety', class: 'status-error' }
  ];
}
