CREATE TABLE [dbo].[Mrc_SampleDocument] (
    [SampleDocumentId]    INT              IDENTITY (1, 1) NOT NULL,
    [Title]               NVARCHAR (100)   NULL,
    [Description]         NVARCHAR (MAX)   NULL,
    [DocumentPath]        NVARCHAR (MAX)   NULL,
    [SampleDevelopmentId] INT              NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SampleDocument] PRIMARY KEY CLUSTERED ([SampleDocumentId] ASC),
    CONSTRAINT [FK_Mrc_SampleDocument_Mrc_SampleDevelopment] FOREIGN KEY ([SampleDevelopmentId]) REFERENCES [dbo].[Mrc_SampleDevelopment] ([SampleDevelopmentId]) ON DELETE CASCADE
);

