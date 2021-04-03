import { Injectable } from '@angular/core';
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of, EMPTY } from 'rxjs';
import { mergeMap, take } from 'rxjs/operators';
import { Task } from '../models/task.model';
import { TaskService } from './tasks.service';
import { NavigationService } from './navigation.service';


@Injectable({
  providedIn: 'root',
})
export class TaskDetailsResolverService implements Resolve<Task> {

  constructor(private cs: TaskService,
              private navigationService: NavigationService) {
  }

  public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Task> | Observable<never> {
    const id = route.paramMap.get('id');
    return this.cs.getTask(id).pipe(
      take(1),
      mergeMap(task => {
        if (task) {
          return of(task);
        } else {
          this.navigationService.goToTasks();
          return EMPTY;
        }
      })
    );
  }
}
