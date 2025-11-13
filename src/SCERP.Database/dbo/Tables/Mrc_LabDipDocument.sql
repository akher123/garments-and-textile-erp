CREATE TABLE [dbo].[Mrc_LabDipDocument] (
    [LabDipDocumentId]    INT              IDENTITY (1, 1) NOT NULL,
    [LabDipDevelopmentId] INT              NOT NULL,
    [Title]               NVARCHAR (100)   NULL,
    [Description]         NVARCHAR (MAX)   NULL,
    [DocumentPath]        NVARCHAR (MAX)   NOT NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_LabDipDocument] PRIMARY KEY CLUSTERED ([LabDipDocumentId] ASC),
    CONSTRAINT [FK_Mrc_LabDipDocument_Mrc_LabDipDevelopment] FOREIGN KEY ([LabDipDevelopmentId]) REFERENCES [dbo].[Mrc_LabDipDevelopment] ([LabDipDevelopmentId])
);

