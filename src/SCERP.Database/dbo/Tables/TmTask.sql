CREATE TABLE [dbo].[TmTask] (
    [TaskId]          INT            IDENTITY (1, 1) NOT NULL,
    [TaskNumber]      NVARCHAR (5)   NULL,
    [ModuleId]        INT            NOT NULL,
    [SubjectId]       INT            NOT NULL,
    [TaskTypeId]      INT            NOT NULL,
    [TaskStatusId]    INT            NOT NULL,
    [AssigneeId]      INT            NOT NULL,
    [TaskName]        NVARCHAR (MAX) NULL,
    [Remarks]         NVARCHAR (MAX) NULL,
    [CompId]          NVARCHAR (3)   NOT NULL,
    [AssignDate]      DATETIME       NULL,
    [EndDate]         DATETIME       NULL,
    [RequirementFile] NVARCHAR (200) NULL,
    [OutPutFile]      NVARCHAR (200) NULL,
    CONSTRAINT [PK_TMTask] PRIMARY KEY CLUSTERED ([TaskId] ASC),
    CONSTRAINT [FK_TmTask_TmModule] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[TmModule] ([ModuleId]),
    CONSTRAINT [FK_TmTask_TmSubject] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[TmSubject] ([SubjectId])
);

