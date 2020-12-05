--------------------------------------------Database---------------------------------------------------------------------------------------------------
use [master]
go

create database [WorkTimeSheet]
go

-------------------------------------------Tables------------------------------------------------------------------------------------------------------

use [WorkTimeSheet]
go

create table [dbo].[Organization]
(
	[ID] [int] identity(1,1) not null,
	[Name] [varchar](100) not null,
	[Address] [varchar](MAX) not null,
	constraint [PK_Organization_ID] primary key clustered ([ID] asc)
)

create table [dbo].[Project]
(
	[ID] [int] identity(1,1) not null,
	[FK_ID_Organization] [int] not null,
	[Name] [varchar](100) not null,
	[Description] [varchar](MAX) not null,
	constraint [PK_Project_ID] primary key clustered ([ID] asc),
	constraint [FK_Project_Organization] foreign key ([FK_ID_Organization]) references [dbo].[Organization] ([ID])
)

create table [dbo].[User]
(
	[ID] [int] identity(1,1) not null,
	[FK_ID_Organization] [int] not null,
	[Name] [varchar](100) not null,
	[Email] [varchar](100) not null,
	[Password] [varchar](100) not null,
	constraint [PK_User_ID] primary key clustered ([ID] asc),
	constraint [UK_User_Email] unique nonclustered([Email] asc),
	constraint [FK_User_Organization] foreign key ([FK_ID_Organization]) references [dbo].[Organization] ([ID])
)

create table [dbo].[ProjectMember]
(
	[ID] [int] identity(1,1) not null,
	[FK_ID_Project] [int] not null,
	[FK_ID_User] [int] not null,
	constraint [PK_ProjectMember_ID] primary key clustered ([ID] asc),
	constraint [UK_ProjectMember_User] unique nonclustered([FK_ID_Project] asc, [FK_ID_User] asc),
	constraint [FK_ProjectMember_Project] foreign key ([FK_ID_Project]) references [dbo].[Project] ([ID]),
	constraint [FK_ProjectMember_User] foreign key ([FK_ID_User]) references [dbo].[User] ([ID])
)

create table [dbo].[UserRole]
(
	[ID] [int] identity(1,1) not null,
	[Role] [varchar](25) not null,
	constraint [PK_UserRole_ID] primary key clustered ([ID] asc),
	constraint [UK_UserRole_Role] unique nonclustered([Role] asc)
)

create table [dbo].[UserRoleMapping]
(
	[ID] [int] identity(1,1) not null,
	[FK_ID_User] [int] not null,
	[FK_ID_UserRole] [int] not null,
	constraint [PK_UserRoleMapping_ID] primary key clustered ([ID] asc),
	constraint [UK_UserRoleMapping_UserRoles] unique nonclustered([FK_ID_User] asc, [FK_ID_UserRole] asc),
	constraint [FK_UserRoleMapping_FK_ID_User] foreign key ([FK_ID_User]) references [dbo].[User] ([ID]),
	constraint [FK_UserRoleMapping_FK_ID_UserRole] foreign key ([FK_ID_UserRole]) references [dbo].[UserRole] ([ID])
)

create table [dbo].[CurrentWork]
(
	[ID] [int] identity(1,1) not null,
	[FK_ID_User] [int] not null,
	[FK_ID_Project] [int] null,
	[StartDateTime] [datetime] null,
	constraint [PK_CurrentWork_ID] primary key clustered ([ID] asc),
	constraint [UK_CurrnetWork_FK_ID_User] unique nonclustered([FK_ID_User] asc),
	constraint [FK_CurrentWork_FK_ID_User] foreign key ([FK_ID_User]) references [dbo].[User] ([ID]),
	constraint [FK_CurrentWork_FK_ID_Project] foreign key ([FK_ID_Project]) references [dbo].[Project] ([ID])
)

create table [dbo].[WorkLog]
(
	[ID] [int] identity(1,1) not null,
	[FK_ID_User] [int] not null,
	[FK_ID_Project] [int] not null,
	[StartDateTime] [datetime] not null,
	[EndDateTime] [datetime] not null,
	[Remarks] [varchar](MAX) null,
	[TimeInSeconds] [bigint] not null,
	constraint [PK_WorkLog_ID] primary key clustered ([ID] asc),
	constraint [FK_WorkLog_User] foreign key ([FK_ID_User]) references [dbo].[User] ([ID]),
	constraint [FK_WorkLog_Project] foreign key ([FK_ID_Project]) references [dbo].[Project] ([ID])
)

Create table [dbo].[RefreshToken]
(
	[ID] [int] identity(1,1) not null,
	[FK_ID_User] [int] not null,
	[RefreshToken] [varchar](MAX) not null,
	[IssueDateTime] [datetime] not null,
	[ExpireDateTime] [datetime] not null,
	constraint [PK_Refreshoken_ID] primary key clustered ([ID] asc),
)

-------------------------------------------Indexes------------------------------------------------------------------------------------------------------

use [WorkTimeSheet]
go

create index [IX_WorkLog_StartDateTime] on [dbo].[WorkLog] ([StartDateTime]);

-------------------------------------------Dafault values------------------------------------------------------------------------------------------------

use [WorkTimeSheet]
go

insert into [dbo].[UserRole] (Role) values ('Owner'),('Project Manager'),('Member')