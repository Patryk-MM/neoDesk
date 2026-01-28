import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import { Observable } from 'rxjs';
import {Ticket, CreateTicket, UpdateTicket, AssignTicket} from '../models/ticket.interface';
import {TicketFilterParams} from "../models/ticket.params";
import {PaginatedResult} from "../models/paginatedresult";

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private apiUrl = '/api/ticket';

  constructor(private http: HttpClient) { }

  getTickets(params: TicketFilterParams) {
    let httpParams: HttpParams = new HttpParams();

    if (params.sortBy) httpParams =  httpParams.set('sortBy', params.sortBy);
    if (params.sortDir) httpParams = httpParams.set('sortDir', params.sortDir);

    if (params.pageIndex) httpParams = httpParams.set('pageIndex', params.pageIndex);
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize);

    if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);

    params.statuses?.forEach(s => httpParams = httpParams.append('statuses', s));
    params.categories?.forEach(c => httpParams = httpParams.append('categories', c));

    return this.http.get<PaginatedResult<Ticket>>(this.apiUrl, {params: httpParams});
  }

  getMyTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.apiUrl}/my-tickets`);
  }

  getTicket(id: number): Observable<Ticket> {
    return this.http.get<Ticket>(`${this.apiUrl}/${id}`);
  }

  createTicket(ticket: CreateTicket): Observable<Ticket> {
    return this.http.post<Ticket>(this.apiUrl, ticket);
  }

  updateTicket(id: number, ticket: UpdateTicket): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, ticket);
  }

  deleteTicket(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  assignTicket(ticketId: number, assignedTo: number | null): Observable<void>{
    return this.http.put<void>(`${this.apiUrl}/${ticketId}/assign`, assignedTo);
  }
}
