import { TaskRecord } from './task-record.model';

export class TaskData {
  constructor(
    public tasks?: TaskRecord[],
    public totalRecords?: number) {
  }
}
