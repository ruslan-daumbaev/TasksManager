export class Task {

    public timeToCompleteActual: string;

    constructor(
        public id?: number,
        public name?: string,
        public description?: string,
        public timeToComplete?: Date,
        public addedDate?: Date,
        public priority?: number,
        public status?: string) {
    }

    resetActualDate() {
        let dt = Math.floor((this.timeToComplete.getTime() - new Date().getTime()) / 1000);

        this.timeToCompleteActual = this.getTimeToComplteString(dt);
    }

    getTimeToComplteString(t: number) {
        if(t <= 0){
            return 'Time is over';
        }
        var days: number, hours: number, minutes: number, seconds: number;
        days = Math.floor(t / 86400);
        t -= days * 86400;
        hours = Math.floor(t / 3600) % 24;
        t -= hours * 3600;
        minutes = Math.floor(t / 60) % 60;
        t -= minutes * 60;
        seconds = t % 60;

        if(days > 0){
            return days + ' day(s) ' +  hours + ' hour(s)';
        }
        if(hours > 0){
            return hours + ' hour(s) ' + minutes + ' minute(s)';
        }
        if(minutes >= 10){
            return minutes + ' minute(s)';
        }

        return [
            minutes + ' min ',
            seconds + ' sec'
        ].join(' ');
    }

}