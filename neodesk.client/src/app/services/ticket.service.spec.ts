import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TicketService } from './ticket.service';
import { Ticket, CreateTicket } from '../models/ticket.interface';

describe('TicketService', () => {
  let service: TicketService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TicketService]
    });
    service = TestBed.inject(TicketService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('createTicket() powinien wysłać żądanie POST z danymi nowego zgłoszenia', () => {
    const newTicketData: CreateTicket = {
      title: 'Problem z drukarką',
      description: 'Nie drukuje na kolorowo',
      category: 1, // Hardware
      status: 0,   // New
      createdByUserId: 1,
      createdAt: '2024-01-01T10:00:00'
    };

    const expectedResponse: Ticket = {
      id: 100,
      title: 'Problem z drukarką',
      description: 'Nie drukuje na kolorowo',
      category: 'Hardware', // Backend zwraca nazwę
      status: 'New',
      createdBy: 'Test User',
      assignedTo: 'Tech',
      createdAt: '2024-01-01T10:00:00'
    };

    service.createTicket(newTicketData).subscribe(response => {
      expect(response).toEqual(expectedResponse);
    });

    const req = httpMock.expectOne('/api/ticket');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newTicketData);

    req.flush(expectedResponse);
  });

  it('updateTicket() powinien wysłać żądanie PUT ze zaktualizowanymi danymi', () => {
    const ticketId = 123;
    const updateData: CreateTicket = {
      title: 'Zaktualizowany tytuł',
      description: 'Nowy opis problemu',
      category: 0, // Software
      status: 1,   // Assigned
      createdByUserId: 1,
      createdAt: '2024-01-01T10:00:00'
    };

    // Wywołanie metody (update zazwyczaj nie zwraca treści lub zwraca void)
    service.updateTicket(ticketId, updateData).subscribe(response => {
      expect(response).toBeNull(); // Zakładając, że backend zwraca 204 No Content
    });

    // Przechwycenie żądania - zwróć uwagę na ID w URL
    const req = httpMock.expectOne(`/api/ticket/${ticketId}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updateData);

    // Symulacja odpowiedzi pustej (204 No Content)
    req.flush(null);
  });


  it('getTicket() powinien pobrać dane zgłoszenia po ID', () => {
    const ticketId = 1;
    const mockTicket: Ticket = {
      id: 1,
      title: 'Test',
      description: 'Desc',
      category: 'Software',
      status: 'New',
      createdAt: '2024-01-01',
      createdBy: 'User',
      assignedTo: 'Tech'
    };

    service.getTicket(ticketId).subscribe(ticket => {
      expect(ticket).toEqual(mockTicket);
    });

    const req = httpMock.expectOne(`/api/ticket/${ticketId}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockTicket);
  });

  it('getMyTickets() powinien pobrać listę zgłoszeń użytkownika', () => {
    service.getMyTickets().subscribe();

    const req = httpMock.expectOne('/api/ticket/my-tickets');
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });
});
