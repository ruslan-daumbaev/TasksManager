import { Component, OnInit, HostBinding } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Task } from '../../model/task.model';

@Component({
    selector: 'app-task-details',
    templateUrl: './task-details.component.html'
})


export class TaskDetailsComponent implements OnInit {
    task: Task;

    constructor(
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit() {
        this.route.data
            .subscribe((data: { task: Task }) => {
                this.task = data.task;
            });
    }
}
