import {AfterViewInit, Component, ElementRef, ViewChild} from '@angular/core';
import {
  Chart,
  ArcElement,
  Tooltip,
  Legend,
  DoughnutController,
} from 'chart.js';

Chart.register(
  ArcElement,
  Tooltip,
  Legend,
  DoughnutController
);

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrl: './stats.component.scss'
})
export class StatsComponent implements AfterViewInit {
  @ViewChild('statusChart') statusChartRef!: ElementRef;

  ngAfterViewInit(): void {
    const canvas = this.statusChartRef?.nativeElement;
    const ctx = canvas?.getContext('2d');

    if (!ctx){
      console.error("Canvas context is null", canvas)
    }

    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['Nowy', 'W toku', 'Zamknięty'],
        datasets: [{
          label: 'Liczba zgłoszeń',
          data: [10, 5, 15],
          backgroundColor: ['#22c55e', '#facc15', '#ef4444'],
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            position: 'bottom',
          }
        }
      }
    });
  }
}

