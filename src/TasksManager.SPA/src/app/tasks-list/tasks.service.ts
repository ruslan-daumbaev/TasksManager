import { Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { Task } from '../model/task.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TaskData } from '../model/task-data.model';
import { environment } from '../../environments/environment.prod';


const API_URL = environment.apiUrl;

@Injectable({
    providedIn: 'root',
})
export class TaskService {

    currentTaskChanged: Subject<number> = new Subject<number>();


    constructor(private http: HttpClient) {
    }

    getTasks(page: number, rows: number, statusFilter: string, sortField: string, sortOrder: string) {
        const options = {
            params: new HttpParams().set('page',
                page.toString()).set('pageSize', rows.toString()).set('statusFilter', statusFilter).set('sortField', sortField).set('sortOrder', sortOrder)
        };
        return this.http.get<TaskData>(API_URL + '/api/Tasks/', options);
    }

    getTask(id: number | string) {
        return this.http.get<Task>(API_URL + '/api/Tasks/' + id);
    }

    addTask(value: any) {
        return this.http.post<Task>(API_URL + '/api/Tasks/', value);
    }

    completeTask(id: number) {
        return this.http.put<Task>(API_URL + '/api/Tasks/', { "id": id });
    }

    deleteTask(id: number) {
        return this.http.delete(API_URL + '/api/Tasks/' + id);
    }
}