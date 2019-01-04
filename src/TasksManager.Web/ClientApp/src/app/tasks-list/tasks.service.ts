import { BehaviorSubject, Observable } from 'rxjs';
import { Injectable, Inject } from '@angular/core';
import { Task } from '../model/task.model';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';


@Injectable({
    providedIn: 'root',
})
export class TaskService {
    private tasks$: Observable<Task[]>;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
        this.tasks$ = http.get<Task[]>(baseUrl + 'api/Tasks/');
    }

    getTasks() {
        return this.tasks$;
    }

    getTask(id: number | string) {
        return this.getTasks().pipe(
            map(tasks => tasks.find(task => task.id === +id))
        );
    }

    addTask(name: string) {

    }
}