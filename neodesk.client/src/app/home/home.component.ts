import {Component, OnInit} from '@angular/core';
import {TicketService} from '../services/ticket.service';
import {Ticket} from '../models/ticket.interface';
import {STATUS_CLASSES} from "../models/ticket.constants";
import {SortDirection, SortOptions, TicketFilterParams} from "../models/ticket.params";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public tickets: Ticket[] = [];
  public isLoading = true;
  public title = 'neodesk.client';
  httpParams: TicketFilterParams = {
    sortBy: SortOptions.Id,
    sortDir: SortDirection.Asc
  }

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.getTickets(this.httpParams);
  }

  getTickets(params: TicketFilterParams): void {
    this.isLoading = true;
    this.ticketService.getTickets(params).subscribe({
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

  setSortingParams(sorting: SortOptions): void {
    if (this.httpParams.sortBy === sorting) {
      this.httpParams.sortDir = this.httpParams.sortDir === SortDirection.Asc
        ? SortDirection.Desc
        : SortDirection.Asc;
    } else {

      this.httpParams.sortBy = sorting;
      this.httpParams.sortDir = SortDirection.Asc;
    }

    this.getTickets(this.httpParams);
  }

  refreshTickets(): void {
    this.getTickets(this.httpParams);
  }

  protected readonly STATUS_CLASSES = STATUS_CLASSES;
  protected readonly SortOptions = SortOptions;
  protected readonly SortDirection = SortDirection;
}
