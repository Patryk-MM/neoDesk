import {ActivatedRoute, Router} from "@angular/router";
import {TicketService} from "../services/ticket.service";

export enum TicketStatus {
  New = 0,
  Assigned = 1,
  Suspended = 2,
  Solved = 3
}

export enum TicketCategory {
  Software = 0,
  Hardware = 1
}

// Helper list for the dropdown
export const statusOptions: { value: TicketStatus; label: string }[] = [
  { value: TicketStatus.New, label: 'Nowy' },
  { value: TicketStatus.Assigned, label: 'Przypisany' },
  { value: TicketStatus.Suspended, label: 'Wstrzymany' },
  { value: TicketStatus.Solved, label: 'Rozwiązany' }
];

export const categoryOptions: {value: TicketCategory; label: string }[] = [
  {value: TicketCategory.Software, label: 'Oprogramowanie' },
  {value: TicketCategory.Hardware, label: 'Sprzęt' },
]

export const getStatusLabel = (statusValue: TicketStatus): string => {
  return statusOptions.find(opt => opt.value === statusValue)?.label || 'Unknown';
}

export const getCategoryLabel = (categoryValue: TicketCategory): string => {
  return categoryOptions.find(opt => opt.value === categoryValue)?.label || 'Unknown';
}
