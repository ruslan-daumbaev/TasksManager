import { Component, OnInit } from '@angular/core';
import {
  Validators,
  FormControl,
  FormGroup,
} from '@angular/forms';
import { Router } from '@angular/router';
import { Message } from 'primeng/api';
import { take } from 'rxjs/operators';
import { TaskService } from '../../services/tasks.service';

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html',
})
export class AddTaskComponent implements OnInit {
  public taskForm: FormGroup;
  public minDate = new Date();
  public messages: Message[] = [];

  constructor(
    private router: Router,
    private taskService: TaskService) {
  }

  public ngOnInit(): void {
    this.taskForm = new FormGroup({
      name: new FormControl('', Validators.required),
      description: new FormControl(''),
      priority: new FormControl(1, Validators.compose([Validators.required, Validators.min(1)])),
      timeToComplete: new FormControl(null, Validators.required),
    });
  }

  public save(form: any): void {
    this.clear();
    this.taskService.addTask(form.value).pipe(take(1)).subscribe(
      (result) => this.router.navigate(['/tasks-list/' + result.id]),
      (error) => this.showError(error)
    );
  }

  public showError(error): void {
    this.messages = [];
    this.messages.push({
      severity: 'error',
      summary: 'Operation failed',
      detail: `Couldn\'t add task: ${ error.statusText }`
    });
  }

  public clear(): void {
    this.messages = [];
  }
}
