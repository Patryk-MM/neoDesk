import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

interface Ticket {
  id: number;
  title: string;
  description: string;
  createdAt: string;
  category: string;
  status: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public tickets: Ticket[] = [];
  public title = 'neodesk.client';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getTickets();
  }

  getTickets(): void {
    this.http.get<Ticket[]>(`${environment.baseUrl}api/ticket`)
      .subscribe({
        next: (result) => this.tickets = result,
        error: (err) => console.error(err)
      });
  }
}
