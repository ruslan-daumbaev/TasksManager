import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddTaskComponent } from './add-task/add-task.component';


const appRoutes: Routes = [
    {
        path: 'add-task',
        component: AddTaskComponent
    },
    {
        path: 'tasks-list',
        loadChildren: './tasks-list/tasks-list.module#TasksListModule',
        data: { preload: true }
    },
    { path: '', redirectTo: '/tasks-list', pathMatch: 'full' }
];

@NgModule({
    imports: [
        RouterModule.forRoot(
            appRoutes,
            {
                enableTracing: false, // <-- debugging purposes only
            }
        )
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule { }
