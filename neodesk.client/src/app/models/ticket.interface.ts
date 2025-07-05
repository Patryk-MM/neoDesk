export interface Ticket {
  id: number;
  title: string;
  description: string;
  createdAt: string;
  category: string;
  status: string;
  createdBy?: string;
  assignedTo?: string;
}

export interface CreateTicket {
  title: string;
  description: string;
  category: number; // 0 = Software, 1 = Hardware
  status: number;   // 0 = New, 1 = Assigned, etc.
  createdByUserId: number;
  createdAt?: string; // Optional - will be set by user or default to now
}
