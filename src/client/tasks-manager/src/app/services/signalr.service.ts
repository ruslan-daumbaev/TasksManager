import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment.prod';
import { TaskChangeEvent } from '../models/task-change.model';

const API_URL = environment.apiUrl;

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private connectionEstablished = new EventEmitter<boolean>();
  private connectionIsEstablished = false;
  private hubConnection: HubConnection;

  constructor() {
    this.createConnection();
  }

  public startConnection(): void {
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

  public stopConnection(): void {
    this.hubConnection.stop();
  }

  public notify(event: TaskChangeEvent): void {
    this.hubConnection.invoke('Notify', event).catch(err => console.error(err.toString()));
  }

  public registerOnServerEvents(callback: (n: TaskChangeEvent) => any): void {
    this.hubConnection.on('Notify', (data: TaskChangeEvent) => {
      callback(data);
    });
  }

  private createConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(API_URL + '/tasks-ws')
      .build();
  }
}
