USE [master]
GO

CREATE DATABASE [TasksDb]

GO

USE [TasksDb]

GO

CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[Priority] [tinyint] NOT NULL,
	[TimeToComplete] [datetime2](7) NOT NULL,
	[AddedDate] [datetime2](7) NOT NULL,
	[ChangeDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
	[Status] [int] NOT NULL)

GO

 
