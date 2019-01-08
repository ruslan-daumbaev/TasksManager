import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Task } from '../../model/task.model';
import { TaskService } from '../tasks.service';

@Component({
    selector: 'task-details',
    templateUrl: './task-details.component.html'
})


export class TaskDetailsComponent implements OnInit {
    task: Task;
    private eventsSubscription: any

    constructor( private route: ActivatedRoute, taskService: TaskService) {
        this.eventsSubscription = taskService.currentTaskChanged.subscribe(() => {
            if (this.task) this.task.status = "Completed";
        });
    }

    ngOnInit() {
        this.route.data
            .subscribe((data: { task: Task }) => {
                this.task = data.task;
            });
    }

    ngOnDestroy() {
        this.eventsSubscription.unsubscribe()
    }
}
