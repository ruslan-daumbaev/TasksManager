# TasksManager

System requirements:
- MS SQL Server (tested on 2017)
- .NET Core SDK 2.2
- Angular 7.X (with CLI)

How to run app:
- download sources into some folder from GitHub using command: 
```
git clone https://github.com/ruslan-daumbaev/TasksManager.git
```
- navigate inside folder TasksManager and switch to branch: 
```
git checkout develop
```
- navigate to folder TasksManager/src and execute script init_database.sql in order to create new database (use SQL Server Management Studio or sqlcmd)
- open cmd (or PowerShell) window and navigate to folder TasksManager\src\TasksManager.WebAPI
- run command (replace server_address, user and password in connection string):
```
dotnet run ConnectionStrings:TMConnectionString="Data Source=<server_address>;Initial Catalog=TasksDb;User ID=<user>;Password=<password>"
```
- wait until ASP.NET Core application is initialized (should be no errors in shell window)
- open other cmd (or PowerShell) window and navigate to folder TasksManager\src\TasksManager.SPA
- run command:
```
ng serve
```
- wait until Angular modules are compiled 
- open browser and go to http://localhost:4200
