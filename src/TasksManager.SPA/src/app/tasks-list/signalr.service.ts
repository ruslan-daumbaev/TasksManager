import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { TaskChangeEvent } from '../model/task-change.model';
import { environment } from '../../environments/environment.prod';

const API_URL = environment.apiUrl;


@Injectable({
    providedIn: 'root',
})
export class SignalRService {
    messageReceived = new EventEmitter<TaskChangeEvent>();
    connectionEstablished = new EventEmitter<Boolean>();

    private connectionIsEstablished = false;
    private _hubConnection: HubConnection;

    constructor() {
        this.createConnection();      
    }

    private createConnection() {
        this._hubConnection = new HubConnectionBuilder()
            .withUrl(API_URL + '/tasks-ws')
            .build();
    }

    startConnection(): void {
        this._hubConnection
            .start()
            .then(() => {
                this.connectionIsEstablished = true;
                console.log('Hub connection started');
                this.connectionEstablished.emit(true);
            })
            .catch(err => {
                console.log('Error while establishing connection, retrying...');
            });
    }

    registerOnServerEvents(callback: (n: TaskChangeEvent) => any): void {
        this._hubConnection.on('Notify', (data: TaskChangeEvent) => {
            callback(data);
        });
    }
}  