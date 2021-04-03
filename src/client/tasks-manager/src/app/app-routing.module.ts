import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddTaskComponent } from './components/add-task/add-task.component';
import { TasksTableComponent } from './components/tasks-table/tasks-table.component';
import { TasksListComponent } from './components/tasks-list/tasks-list.component';
import { TaskDetailsComponent } from './components/task-details/task-details.component';
import { TaskDetailsResolverService } from './services/task-details-resolver.service';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/tasks-list',
    pathMatch: 'full'
  },
  {
    path: 'add-task',
    component: AddTaskComponent
  },
  {
    path: 'tasks-list',
    component: TasksListComponent,
    children: [
      {
        path: '',
        component: TasksTableComponent,
        children: [
          {
            path: ':id',
            component: TaskDetailsComponent,
            resolve: {
              task: TaskDetailsResolverService
            }
          }
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
