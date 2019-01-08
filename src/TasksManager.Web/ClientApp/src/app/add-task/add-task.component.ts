import { Component, OnInit } from '@angular/core';
import { Validators, FormControl, FormGroup, FormBuilder, NgForm } from "@angular/forms";
import { Task } from "../model/task.model";
import { Router } from "@angular/router";
import { TaskService } from '../tasks-list/tasks.service';
import { Message } from 'primeng/components/common/api';

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html'
})
export class AddTaskComponent implements OnInit {
  task: Task = new Task();
  taskform: FormGroup;
  minDate: Date;
  msgs: Message[] = [];


  constructor(private fb: FormBuilder, private router: Router, private taskService: TaskService) {
    this.minDate = new Date();
  }

  ngOnInit() {
    this.taskform = this.fb.group({
      'name': new FormControl('', Validators.required),
      'description': new FormControl(''),
      'priority': new FormControl(1, Validators.compose([Validators.required, Validators.min(1)])),
      'timeToComplete': new FormControl('', Validators.required)
    });
  }

  save(form: NgForm) {
    this.clear();
    this.taskService.addTask(form.value).subscribe(result => {
      this.router.navigate(['/tasks-list/' + result.id]);
    }, error => {
      console.error(error);
      this.showError(error);
    });
  }

  showError(error) {
    this.msgs = [];
    this.msgs.push({ severity: 'error', summary: 'Operation failed', detail: 'Couldn\'t add task: ' + error.statusText });
  }

  clear() {
    this.msgs = [];
  }
}
