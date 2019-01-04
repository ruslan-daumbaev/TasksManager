export class Task {
    constructor(
        public id?: number,
        public name?: string,
        public description?: string,
        public timeToComplete?: Date,
        public priority?: number,
        public status?: string) { }
}