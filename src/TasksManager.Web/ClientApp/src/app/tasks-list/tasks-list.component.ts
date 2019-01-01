import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-tasks-list',
  templateUrl: './tasks-list.component.html'
})
export class TasksListComponent {
  public forecasts: Task[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Task[]>(baseUrl + 'api/Tasks/WeatherForecasts').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

interface Task {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
