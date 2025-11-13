CREATE TABLE [dbo].[Mrc_KeyProcess] (
    [KeyProcessId]           INT              IDENTITY (1, 1) NOT NULL,
    [KeyProcessName]         NVARCHAR (100)   NOT NULL,
    [ProcessingDepartmentId] INT              NOT NULL,
    [BufferDay]              INT              NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_KeyProcess] PRIMARY KEY CLUSTERED ([KeyProcessId] ASC),
    CONSTRAINT [FK_Mrc_KeyProcess_Mrc_ProcessingDepartment] FOREIGN KEY ([ProcessingDepartmentId]) REFERENCES [dbo].[Mrc_ProcessingDepartment] ([ProcessingDepartmentId]) ON DELETE CASCADE
);

