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
    private hubConnection: HubConnection;

    constructor() {
        this.createConnection();
    }

    private createConnection() {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(API_URL + '/tasks-ws')
            .build();
    }

    startConnection(): void {
        this.hubConnection
            .start()
            .then(() => {
                this.connectionIsEstablished = true;
                console.log('Hub connection started');
                this.connectionEstablished.emit(true);
            })
            .catch(err => {
                console.log(err);
                console.log('Error while establishing connection, retrying...');
                setTimeout(() => {
                    this.startConnection();
                }, 5000);
            });
    }

    stopConnection(): void {
        this.hubConnection.stop();
    }

    notify(event: TaskChangeEvent){
        this.hubConnection.invoke("Notify", event).catch(err => console.error(err.toString()));
    }

    registerOnServerEvents(callback: (n: TaskChangeEvent) => any): void {
        this.hubConnection.on("Notify", (data: TaskChangeEvent) => {
            callback(data);
        });
    }
}  