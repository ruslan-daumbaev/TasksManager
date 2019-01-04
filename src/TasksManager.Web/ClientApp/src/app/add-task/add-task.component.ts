import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgForm } from "@angular/forms";
import { Task } from "../model/task.model";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html'
})
export class AddTaskComponent {
  task: Task = new Task();


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, activeRoute: ActivatedRoute, private router: Router) {

  }

  save(form: NgForm) {
    this.http.post<Task>(this.baseUrl + 'api/Tasks/', this.task).subscribe(result => {
      this.task = result;
      this.router.navigateByUrl("/tasks-list/" + result.id);
    }, error => console.error(error));
  }
}
