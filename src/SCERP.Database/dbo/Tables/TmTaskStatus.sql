CREATE TABLE [dbo].[TmTaskStatus] (
    [TaskStatusId] INT           IDENTITY (1, 1) NOT NULL,
    [TaskStatus]   NVARCHAR (50) NOT NULL,
    [CompId]       NVARCHAR (3)  NULL,
    CONSTRAINT [PK_TmTaskStatus] PRIMARY KEY CLUSTERED ([TaskStatusId] ASC)
);

