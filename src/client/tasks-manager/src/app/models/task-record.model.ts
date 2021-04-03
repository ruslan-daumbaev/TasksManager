import { TaskStatus } from './task-status.enum';

export class TaskRecord {
    constructor(
        public id?: number,
        public name?: string,
        public timeToComplete?: string,
        public addedDate?: Date,
        public priority?: number,
        public status?: TaskStatus) {
    }
}
