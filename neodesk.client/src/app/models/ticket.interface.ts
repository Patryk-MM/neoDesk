import {TicketCategory, TicketStatus} from "./ticket.enums";
import {SimpleUserDTO} from "./user.interface";
import {Comment} from './comment.interface';

export interface Ticket {
  id: number;
  title: string;
  description: string;
  createdAt: string;
  category: TicketCategory;
  status: TicketStatus;
  createdBy?: SimpleUserDTO;
  assignedTo?: SimpleUserDTO | null;
  comments: Comment[];
}

export interface CreateTicket {
  title: string;
  description: string;
  category: TicketCategory;
  status: TicketStatus;
  createdByUserId: number;
  createdAt?: string;
}

export interface UpdateTicket {
  title: string;
  description: string;
  category: TicketCategory;
  status: TicketStatus;
}

export interface AssignTicket {
  assignedToUserId?: number | null;
}
