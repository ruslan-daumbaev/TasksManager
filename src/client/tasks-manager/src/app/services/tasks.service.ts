import { Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment.prod';
import { TaskData } from '../models/task-data.model';
import { Task } from '../models/task.model';

const API_URL = environment.apiUrl;

@Injectable({
    providedIn: 'root',
})
export class TaskService {
    public currentTaskChanged: Subject<number> = new Subject<number>();

    constructor(private http: HttpClient) {
    }

    public getTasks(page: number, rows: number, statusFilter: string, sortField: string, sortOrder: string): Observable<TaskData> {
        const options = {
            params: new HttpParams().set('page', page.toString())
                                    .set('pageSize', rows.toString())
                                    .set('statusFilter', statusFilter)
                                    .set('sortField', sortField)
                                    .set('sortOrder', sortOrder)
        };
        return this.http.get<TaskData>(this.apiUrl, options);
    }

    public getTask(id: number | string): Observable<Task> {
        return this.http.get<Task>(`${this.apiUrl}${id}`);
    }

    public addTask(value: any): Observable<Task> {
        return this.http.post<Task>(this.apiUrl, value);
    }

    public completeTask(id: number): Observable<Task>  {
        return this.http.put<Task>(this.apiUrl, { id });
    }

    public deleteTask(id: number): Observable<object>  {
        return this.http.delete(`${this.apiUrl}${id}`);
    }

    private get apiUrl(): string {
      return `${API_URL}/api/Tasks/`;
    }
}
