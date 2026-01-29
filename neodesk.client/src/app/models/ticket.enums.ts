export enum TicketStatus {
  New = 'New',
  Assigned = 'Assigned',
  Suspended = 'Suspended',
  Solved = 'Solved',
}

export enum TicketCategory {
  Software = 'Software',
  Hardware = 'Hardware',
}

// Helper list for the dropdown
export const statusOptions: { value: TicketStatus; label: string }[] = [
  { value: TicketStatus.New, label: 'Nowe' },
  { value: TicketStatus.Assigned, label: 'W realizacji' },
  { value: TicketStatus.Suspended, label: 'Zawieszone' },
  { value: TicketStatus.Solved, label: 'Rozwiązane' }
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
