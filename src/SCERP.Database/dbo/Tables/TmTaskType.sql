CREATE TABLE [dbo].[TmTaskType] (
    [TaskTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [TaskType]   NVARCHAR (50) NOT NULL,
    [CompId]     NVARCHAR (3)  NULL,
    CONSTRAINT [PK_TmTaskType] PRIMARY KEY CLUSTERED ([TaskTypeId] ASC)
);

