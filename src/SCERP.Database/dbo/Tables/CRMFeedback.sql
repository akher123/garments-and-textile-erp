CREATE TABLE [dbo].[CRMFeedback] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [Subject]        NVARCHAR (200)   NOT NULL,
    [Description]    NVARCHAR (MAX)   NULL,
    [JobStatus]      INT              NULL,
    [PhotographPath] NVARCHAR (200)   NULL,
    [ModuleId]       INT              NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED ([Id] ASC)
);

