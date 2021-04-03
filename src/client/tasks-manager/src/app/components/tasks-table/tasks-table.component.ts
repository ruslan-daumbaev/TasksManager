import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'primeng/api/message';
import { ReplaySubject, timer } from 'rxjs';
import { TaskChangeEvent } from 'src/app/models/task-change.model';
import { Task } from 'src/app/models/task.model';
import { NotificationsService } from 'src/app/services/notifications.service';
import { TaskService } from 'src/app/services/tasks.service';
import { TaskStatus } from '../../models/task-status.enum';
import { take, takeUntil } from 'rxjs/operators';
import { NavigationService } from '../../services/navigation.service';

@Component({
  selector: 'app-tasks-table',
  templateUrl: './tasks-table.component.html',
})
export class TasksTableComponent implements OnInit, OnDestroy {
  private subscription$ = new ReplaySubject<any>(1);
  public messages: Message[] = [];
  public selectedId: number;
  public tasks: Task[];
  public selectedTask: Task;
  public statuses: any[];
  public selectedFilter: string;
  public totalRecords = 100;
  public loading = false;
  public currentFirst: number;
  public currentRows: number;

  constructor(
    private taskService: TaskService,
    private activeRoute: ActivatedRoute,
    private notificationsService: NotificationsService,
    private navigationService: NavigationService
  ) {
  }

  public ngOnInit(): void {
    this.statuses = [
      { label: 'All', value: null },
      { label: 'Active', value: 'Active' },
      { label: 'Completed', value: 'Completed' },
    ];

    if (this.activeRoute.firstChild && this.activeRoute.firstChild.params) {
      this.activeRoute.firstChild.params.pipe(takeUntil(this.subscription$)).subscribe(params => (this.selectedId = params.id));
    }

    timer(0, 1000).pipe(takeUntil(this.subscription$)).subscribe(() => this.tasks?.forEach(task => task.resetActualDate()));

    this.notificationsService.subscribe(event => this.onTaskChanged(event));
    this.notificationsService.startListening();
  }


  public ngOnDestroy(): void {
    this.subscription$.next(null);
    this.subscription$.complete();
    this.notificationsService.stopListening();
  }

  public onRowSelect(event: { data: Task }): void {
    this.navigationService.goToTask(event.data.id);
  }

  public onRowUnselect(): void {
    this.navigationService.goToTasks();
  }

  public update(dataTable: any): void {
    this.clear();
    dataTable.reset();
  }

  public loadDataOnScroll(event): void {
    this.loading = true;
    this.currentFirst = event.first;
    this.currentRows = event.rows;
    const select = this.selectedId;
    this.taskService
      .getTasks(
        event.first,
        event.rows,
        event.filters?.global?.value,
        event.sortField,
        event.sortOrder
      ).pipe(take(1))
      .subscribe(
        data => {
          this.tasks = data.tasks.map(ts => new Task().fromRecord(ts));
          this.totalRecords = data.totalRecords;
          this.loading = false;
          this.selectedFilter = event.globalFilter;
          if (!this.selectedTask) {
            this.selectedTask = this.tasks.find((task) => task.id === select);
          }
        },
        (error) => {
          this.showError(error);
        }
      );
  }

  public completeTask(event, rowData): void {
    this.selectedTask = rowData;
    this.navigationService.goToTask(rowData.id);
    this.taskService.completeTask(rowData.id).pipe(take(1)).subscribe(
      () => {
        rowData.status = TaskStatus.Completed;
        this.taskService.currentTaskChanged.next(rowData.id);
        this.notificationsService.notify(new TaskChangeEvent(rowData.id, TaskStatus.Completed));
      },
      (error) => {
        this.showError(error);
      }
    );
  }

  public removeTask(event, rowData): void {
    this.selectedTask = rowData;
    this.taskService.deleteTask(rowData.id).pipe(take(1)).subscribe(
      () => {
        const task = this.tasks.find((t) => t.id === rowData.id);
        const index = this.tasks.indexOf(task);
        this.tasks.splice(index, 1);
        this.selectedTask = null;
        this.navigationService.goToTasks();
        this.notificationsService.notify(new TaskChangeEvent(rowData.id, TaskStatus.Deleted));
      },
      (error) => {
        this.showError(error);
      }
    );
  }

  private showError(error): void {
    this.messages = [];
    this.messages.push({
      severity: 'error',
      summary: 'Operation failed',
      detail: `Error: ${error.statusText}`,
    });
  }

  private onTaskChanged(event: TaskChangeEvent): void {
    if (event.change === TaskStatus.Completed) {
      const changedTask = this.tasks.find((task) => task.id === event.id);
      if (changedTask) {
        changedTask.status = TaskStatus.Completed;
      }
      if (this.selectedTask && this.selectedTask.id === event.id) {
        this.taskService.currentTaskChanged.next(event.id);
      }
    }
    if (event.change === TaskStatus.Deleted) {
      const changedTask = this.tasks.find((task) => task.id === event.id);
      if (changedTask) {
        const index = this.tasks.indexOf(changedTask);
        this.tasks.splice(index, 1);
      }
      if (this.selectedTask && this.selectedTask.id === event.id) {
        this.selectedTask = null;
        this.navigationService.goToTasks();
      }
    }
  }

  private clear(): void {
    this.messages = [];
  }
}
