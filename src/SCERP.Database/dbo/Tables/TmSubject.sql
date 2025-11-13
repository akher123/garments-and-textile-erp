CREATE TABLE [dbo].[TmSubject] (
    [SubjectId]   INT            IDENTITY (1, 1) NOT NULL,
    [SubjectName] NVARCHAR (50)  NOT NULL,
    [ModuleId]    INT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [CompId]      VARCHAR (3)    NOT NULL,
    CONSTRAINT [PK_TMSubject] PRIMARY KEY CLUSTERED ([SubjectId] ASC),
    CONSTRAINT [FK_TmSubject_TmModule] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[TmModule] ([ModuleId])
);

