import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from "@angular/router";
import { Task } from '../../model/task.model';
import { TaskService } from '../tasks.service';
import { DataTable } from 'primeng/datatable';
import { timer, Subscription } from 'rxjs';
import { Message } from 'primeng/components/common/api';
import { MessageService } from 'primeng/components/common/messageservice';
import { SignalRService } from '../signalr.service';

@Component({
    selector: 'app-tasks-table',
    templateUrl: './tasks-table.component.html'
})
export class TasksTableComponent implements OnInit {
    msgs: Message[] = [];

    selectedId: number;

    tasks: Task[];

    selectedTask: Task;

    filters: FilterType[];

    selectedFilter: string;

    totalRecords: number;

    loading: boolean = false;

    currentFirst: number;

    currentRows: number;

    params: Subscription;

    constructor(private taskService: TaskService,
        private activeRoute: ActivatedRoute, private router: Router, private signalr: SignalRService) {

        this.filters = [
            { label: 'All', value: null },
            { label: 'Active', value: 'Active' },
            { label: 'Completed', value: 'Completed' }
        ];
        this.totalRecords = 100;
        this.signalr.registerOnServerEvents((p) => {
            if (p.change === "Completed") {
                const changedTask = this.tasks.find(task => task.id == p.id);
                if (changedTask) {
                    changedTask.status = "Completed";
                }
                if(this.selectedTask && this.selectedTask.id === p.id){
                    this.taskService.currentTaskChanged.next(p.id); 
                }
            }
            if (p.change === "Deleted") {
                const changedTask = this.tasks.find(task => task.id == p.id);
                if (changedTask) {
                    const index = this.tasks.indexOf(changedTask);
                    this.tasks.splice(index, 1);
                }
                if(this.selectedTask && this.selectedTask.id === p.id){
                    this.selectedTask = null;
                    this.router.navigate(['/tasks-list/']);
                }
            }
        });
    }

    ngOnInit() {
        if (this.activeRoute.firstChild && this.activeRoute.firstChild.params) {
            this.params = this.activeRoute.firstChild.params.subscribe(params => this.selectedId = params['id']);
        }
        this.selectedFilter = null;
        timer(0, 1000).subscribe(val => {
            if (this.tasks) {
                for (let task of this.tasks) {
                    task.resetActualDate();
                }
            }
        });


        this.signalr.startConnection();
    }

    ngOnDestroy() {
        if (this.params) {
            this.params.unsubscribe();
        }
    }

    onRowSelect(event: { data: Task; }) {
        console.debug(event.data.id);
        this.router.navigate(['/tasks-list/' + event.data.id]);
    }

    onRowUnselect(event) {
        console.debug(event.data.id);
        this.router.navigate(['/tasks-list']);
    }

    update(dt: DataTable) {
        this.clear();
        dt.reset();
    }

    loadDataOnScroll(event) {
        this.loading = true;
        this.currentFirst = event.first;
        this.currentRows = event.rows;
        let select = this.selectedId;
        this.taskService.getTasks(event.first, event.rows, event.globalFilter, event.sortField, event.sortOrder).subscribe(t => {
            this.tasks = t.tasks.map(ts => new Task(ts.id, ts.name, null, new Date(ts.timeToComplete), ts.addedDate, ts.priority, ts.status));
            this.totalRecords = t.totalRecords;
            this.loading = false;
            this.selectedFilter = event.globalFilter;
            if (this.selectedTask == undefined) {
                this.selectedTask = this.tasks.find(task => task.id == select);
            }
        }, error => {
            console.error(error);
            this.showError(error);
        });
    }

    completeTask(event, rowData) {
        this.selectedTask = rowData;
        this.router.navigate(['/tasks-list/' + rowData.id]);
        this.taskService.completeTask(rowData.id).subscribe(result => {
            rowData.status = "Completed";
            this.taskService.currentTaskChanged.next(rowData.id);
        }, error => {
            console.error(error);
            this.showError(error);
        });
    }

    removeTask(event, rowData) {
        this.selectedTask = rowData;
        this.taskService.deleteTask(rowData.id).subscribe(result => {
            let task = this.tasks.find(t => t.id == rowData.id);
            const index = this.tasks.indexOf(task);
            this.tasks.splice(index, 1);
            this.selectedTask = null;
            this.router.navigate(['/tasks-list/']);
        }, error => {
            console.error(error);
            this.showError(error);
        });
        console.debug(this.selectedTask.id);
    }

    showError(error) {
        this.msgs = [];
        this.msgs.push({ severity: 'error', summary: 'Operation failed', detail: 'Error: ' + error.statusText });
    }

    clear() {
        this.msgs = [];
    }
}

interface FilterType {
    label: string;
    value: string;
}
