CREATE TABLE [dbo].[TmAssignee] (
    [AssigneeId] INT           IDENTITY (1, 1) NOT NULL,
    [Assignee]   VARCHAR (50)  NOT NULL,
    [CompId]     NVARCHAR (50) NULL,
    CONSTRAINT [PK_TaskAssignee] PRIMARY KEY CLUSTERED ([AssigneeId] ASC)
);

