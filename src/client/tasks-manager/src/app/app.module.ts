import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddTaskComponent } from './components/add-task/add-task.component';
import { TaskDetailsComponent } from './components/task-details/task-details.component';
import { TasksListComponent } from './components/tasks-list/tasks-list.component';
import { TasksTableComponent } from './components/tasks-table/tasks-table.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { SpinnerModule } from 'primeng/spinner';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { PanelModule } from 'primeng/panel';
import { ToolbarModule } from 'primeng/toolbar';
import { SelectButtonModule } from 'primeng/selectbutton';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { EditorModule } from 'primeng/editor';
import { ToastModule } from 'primeng/toast';
import { TaskService } from './services/tasks.service';
import { NotificationsService } from './services/notifications.service';
import { TaskDetailsResolverService } from './services/task-details-resolver.service';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DropdownModule } from 'primeng/dropdown';
import { NavigationService } from './services/navigation.service';

@NgModule({
  declarations: [
    AppComponent,
    AddTaskComponent,
    TaskDetailsComponent,
    TasksListComponent,
    TasksTableComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    TableModule,
    ReactiveFormsModule,
    MessagesModule,
    MessageModule,
    EditorModule,
    ToastModule,
    CalendarModule,
    InputTextModule,
    InputTextareaModule,
    SpinnerModule,
    ButtonModule,
    TableModule,
    PanelModule,
    ToolbarModule,
    SelectButtonModule,
    AppRoutingModule,
    ReactiveFormsModule,
    MessagesModule,
    MessageModule,
    EditorModule,
    ToastModule,
    DropdownModule
  ],
  providers: [
    TaskService,
    NotificationsService,
    TaskDetailsResolverService,
    NavigationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
