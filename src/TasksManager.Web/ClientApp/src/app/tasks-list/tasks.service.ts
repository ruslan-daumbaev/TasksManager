import { Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { Task } from '../model/task.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TaskData } from '../model/task-data.model';


@Injectable({
    providedIn: 'root',
})
export class TaskService {
    private tasks$: Observable<TaskData>;

    currentTaskChanged: Subject<number> = new Subject<number>();


    constructor(private http: HttpClient) {
    }

    getTasks(page: number, rows: number, statusFilter: string) {
        const options = { params: new HttpParams().set('page', page.toString()).set('pageSize', rows.toString()).set('statusFilter', statusFilter) };
        this.tasks$ = this.http.get<TaskData>('api/Tasks/', options);
        return this.tasks$;
    }

    getTask(id: number | string) {
        return this.http.get<Task>('api/Tasks/' + id);
    }

    addTask(value: any) {
        return this.http.post<Task>('api/Tasks/', value);
    }

    completeTask(id: number) {
        return this.http.put<Task>('api/Tasks/', { "id": id });
    }

    deleteTask(id: number) {
        return this.http.delete('api/Tasks/' + id);
    }
}
