import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class NavigationService {

  constructor(private router: Router) {
  }

  public goToTask(id: number): void {
    this.router.navigate(['/tasks-list/' + id]);
  }

  public goToTasks(): void {
    this.router.navigate(['/tasks-list']);
  }
}
