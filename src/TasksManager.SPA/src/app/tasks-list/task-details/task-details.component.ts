import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Task } from '../../model/task.model';
import { TaskService } from '../tasks.service';
import { SafeHtml, DomSanitizer } from '@angular/platform-browser';

@Component({
    selector: 'task-details',
    templateUrl: './task-details.component.html'
})


export class TaskDetailsComponent implements OnInit {
    task: Task;
    private eventsSubscription: any
    myStyle: SafeHtml;
    description: SafeHtml;

    constructor(private sanitizer: DomSanitizer, private route: ActivatedRoute, taskService: TaskService) {
        this.eventsSubscription = taskService.currentTaskChanged.subscribe(() => {
            if (this.task) {
                this.task.status = "Completed";
            }
        });
    }

    ngOnInit() {
        this.route.data
            .subscribe((data: { task: Task }) => {
                this.task = data.task;
                this.description = this.sanitizer.bypassSecurityTrustHtml(this.task.description);
            });
    }

    ngOnDestroy() {
        this.eventsSubscription.unsubscribe()
    }
}
