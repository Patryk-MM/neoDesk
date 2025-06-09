import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];
  public isMinimized = false;
  public title = 'neodesk.client';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getForecasts();
  }

  getForecasts(): void {
    this.http.get<WeatherForecast[]>(`${environment.baseUrl}api/weatherforecast`)
      .subscribe({
        next: (result) => this.forecasts = result,
        error: (err) => console.error(err)
      });
  }
}
