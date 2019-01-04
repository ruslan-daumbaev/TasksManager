import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from "@angular/router";
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Task } from '../../model/task.model';
import { TaskService } from '../tasks.service';

@Component({
    selector: 'app-tasks-table',
    templateUrl: './tasks-table.component.html'
})
export class TasksTableComponent implements OnInit {
    selectedId: number;

    public tasks$: Observable<Task[]>;

    public selectedTask: Task;

    public filters: FilterType[];

    public selectedFilter: FilterType;

    constructor(private taskService: TaskService, 
        private activeRoute: ActivatedRoute, private router: Router) {

        this.filters = [
            { label: 'All', value: null },
            { label: 'Active', value: 'Active' },
            { label: 'Completed', value: 'Completed' }
        ];

        
    }

    ngOnInit() {
        this.tasks$ = this.activeRoute.paramMap.pipe(
            switchMap(params => {
                this.selectedId = +params.get('id');
                return this.taskService.getTasks();
            })
        );
        this.selectedFilter = null;
    }

    onRowSelect(event) {
        console.debug(event.data.id);
        this.router.navigate(['/tasks-list/' + event.data.id]);
    }

    onRowUnselect(event) {
        console.debug(event.data.id);
        this.router.navigate(['/tasks-list']);
    }
}


interface FilterType {
    label: string;
    value: string;
}
