import { TaskStatus } from './task-status.enum';

export class TaskChangeEvent {
  constructor(
    public id?: number,
    public change?: TaskStatus) {
  }
}
