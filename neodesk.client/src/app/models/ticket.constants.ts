import {TicketCategory, TicketStatus} from "./ticket.enums";

export const TICKET_STATUS_LABELS: Record<TicketStatus, string> = {
  [TicketStatus.New]: 'Nowe',
  [TicketStatus.Assigned]: 'W realizacji',
  [TicketStatus.Suspended]: 'Zawieszone',
  [TicketStatus.Solved]: 'Rozwiązane'
}

export const TICKET_CATEGORY_LABELS: Record<TicketCategory, string> = {
  [TicketCategory.Software]: 'Oprogramowanie',
  [TicketCategory.Hardware]: 'Sprzęt'
}

export const STATUS_CLASSES: Record<TicketStatus, string> = {
  [TicketStatus.New]: 'badge-success',
  [TicketStatus.Assigned]: 'badge-warning',
  [TicketStatus.Suspended]: 'badge-error',
  [TicketStatus.Solved]: 'badge-info'
}

export const STATUS_OPTIONS = Object.entries(TICKET_STATUS_LABELS).map(([value, label]) => ({
  value: value as TicketStatus, label,
}))

export const CATEGORY_OPTIONS = Object.entries(TICKET_CATEGORY_LABELS).map(([value, label]) => ({
  value: value as TicketCategory,
  label
}));
