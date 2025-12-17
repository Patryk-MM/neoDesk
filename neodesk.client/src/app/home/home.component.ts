import { Component, OnInit } from '@angular/core';
import { TicketService } from '../services/ticket.service';
import { Ticket } from '../models/ticket.interface';
import {statusOptions, categoryOptions, getStatusLabel, getCategoryLabel} from '../models/ticket.enums'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public tickets: Ticket[] = [];
  public isLoading = true;
  public title = 'neodesk.client';

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.getTickets();
  }

  getTickets(): void {
    this.isLoading = true;
    this.ticketService.getTickets().subscribe({
      next: (result) => {
        this.tickets = result;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  refreshTickets(): void {
    this.getTickets();
  }

  protected readonly statusOptions = statusOptions;
  protected readonly getStatusLabel = getStatusLabel;
  protected readonly getCategoryLabel = getCategoryLabel;
}
