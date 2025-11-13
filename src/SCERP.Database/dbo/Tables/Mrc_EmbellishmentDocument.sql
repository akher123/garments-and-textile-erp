CREATE TABLE [dbo].[Mrc_EmbellishmentDocument] (
    [EmbellishmentDocumentId]    INT              IDENTITY (1, 1) NOT NULL,
    [EmbellishmentDevelopmentId] INT              NOT NULL,
    [Title]                      NVARCHAR (100)   NOT NULL,
    [Description]                NVARCHAR (MAX)   NULL,
    [DocumentPath]               NVARCHAR (MAX)   NOT NULL,
    [CreatedDate]                DATETIME         NULL,
    [CreatedBy]                  UNIQUEIDENTIFIER NULL,
    [EditedDate]                 DATETIME         NULL,
    [EditedBy]                   UNIQUEIDENTIFIER NULL,
    [IsActive]                   BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_EmbellishmentDocument] PRIMARY KEY CLUSTERED ([EmbellishmentDocumentId] ASC),
    CONSTRAINT [FK_Mrc_EmbellishmentDocument_Mrc_EmbellishmentDevelopment] FOREIGN KEY ([EmbellishmentDevelopmentId]) REFERENCES [dbo].[Mrc_EmbellishmentDevelopment] ([EmbellishmentDevelopmentId])
);

