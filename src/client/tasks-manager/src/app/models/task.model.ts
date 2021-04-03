import { TaskStatus } from './task-status.enum';
import { TaskRecord } from './task-record.model';

export class Task {
  public id?: number;
  public name?: string;
  public description?: string;
  public timeToComplete?: Date;
  public timeToCompleteActual: string;
  public addedDate?: Date;
  public priority?: number;
  public status?: TaskStatus;

  constructor() {
  }

  public fromRecord(record: TaskRecord): Task {
    this.id = record.id;
    this.name = record.name;
    this.timeToComplete = new Date(record.timeToComplete);
    this.addedDate = new Date(record.addedDate);
    this.status = record.status;
    this.priority = record.priority;
    return this;
  }

  public fromTask(task: Task): Task {
    Object.assign(this, task);
    this.timeToComplete = new Date(task.timeToComplete);
    this.addedDate = new Date(task.addedDate);
    return this;
  }

  public resetActualDate(): void {
    if (this.status === 'Completed') {
      this.timeToCompleteActual = 'Completed';
      return;
    }
    const dt = Math.floor((this.timeToComplete.getTime() - new Date().getTime()) / 1000);

    this.timeToCompleteActual = this.getTimeToComplteString(dt);
  }

  public getTimeToComplteString(t: number): string {
    if (t <= 0) {
      return 'Time is over';
    }

    const days = Math.floor(t / 86400);
    t -= days * 86400;
    const hours = Math.floor(t / 3600) % 24;
    t -= hours * 3600;
    const minutes = Math.floor(t / 60) % 60;
    t -= minutes * 60;
    const seconds = t % 60;

    if (days > 0) {
      return days + ' day(s) ' + hours + ' hour(s)';
    }
    if (hours > 0) {
      return hours + ' hour(s) ' + minutes + ' minute(s)';
    }
    if (minutes >= 10) {
      return minutes + ' minute(s)';
    }

    return [
      minutes + ' min ',
      seconds + ' sec'
    ].join(' ');
  }
}
