import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Task } from 'src/app/models/task.model';
import { TaskService } from 'src/app/services/tasks.service';
import { TaskStatus } from '../../models/task-status.enum';

@Component({
  selector: 'app-task-details',
  templateUrl: './task-details.component.html'
})
export class TaskDetailsComponent implements OnInit, OnDestroy {
  private eventsSubscription: any;
  public task: Task;
  public myStyle: SafeHtml;
  public description: SafeHtml;

  constructor(private sanitizer: DomSanitizer, private route: ActivatedRoute, taskService: TaskService) {
    this.eventsSubscription = taskService.currentTaskChanged.subscribe(() => {
      if (this.task) {
        this.task.status = TaskStatus.Completed;
      }
    });
  }

  public ngOnInit(): void {
    this.route.data
      .subscribe((data: { task: Task }) => {
        this.task = new Task().fromTask(data.task);
        this.description = this.sanitizer.bypassSecurityTrustHtml(this.task.description);
      });
  }

  public ngOnDestroy(): void {
    this.eventsSubscription.unsubscribe();
  }
}
