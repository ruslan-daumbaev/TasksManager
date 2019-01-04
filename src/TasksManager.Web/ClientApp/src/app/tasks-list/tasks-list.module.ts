import { NgModule }       from '@angular/core';
import { FormsModule }    from '@angular/forms';
import { CommonModule }   from '@angular/common';

import { TasksListComponent }       from './tasks-list/tasks-list.component';
import { TasksTableComponent }       from './tasks-table/tasks-table.component';
import { TaskDetailsComponent }       from './task-details/task-details.component';
import { CalendarModule } from 'primeng/calendar';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { SpinnerModule } from 'primeng/spinner';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { PanelModule } from 'primeng/panel';
import { ToolbarModule } from 'primeng/toolbar';
import { SelectButtonModule } from 'primeng/selectbutton';
import { TasksListRoutingModule } from './tasks-list-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    TasksListRoutingModule,
    CalendarModule,
    InputTextModule,
    InputTextareaModule,
    SpinnerModule,
    ButtonModule,
    TableModule,
    PanelModule,
    ToolbarModule,
    SelectButtonModule
  ],
  declarations: [
    TasksListComponent,
    TasksTableComponent,
    TaskDetailsComponent
  ]
})
export class TasksListModule {}