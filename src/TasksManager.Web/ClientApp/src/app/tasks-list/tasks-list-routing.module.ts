import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TasksListComponent }       from './tasks-list/tasks-list.component';
import { TasksTableComponent }       from './tasks-table/tasks-table.component';
import { TaskDetailsComponent }     from './task-details/task-details.component';
import { TaskDetailsResolverService } from './task-details-resolver.service';

const tasksListRoutes: Routes = [
  {
    path: '',
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
  imports: [
    RouterModule.forChild(tasksListRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class TasksListRoutingModule { }
