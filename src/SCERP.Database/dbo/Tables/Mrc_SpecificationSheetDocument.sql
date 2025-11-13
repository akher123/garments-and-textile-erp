CREATE TABLE [dbo].[Mrc_SpecificationSheetDocument] (
    [DocumentId]           INT              IDENTITY (1, 1) NOT NULL,
    [Title]                NVARCHAR (100)   NULL,
    [Path]                 NVARCHAR (100)   NOT NULL,
    [SpecificationSheetId] INT              NOT NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_MerchandisingSpecificationSheetDocument] PRIMARY KEY CLUSTERED ([DocumentId] ASC),
    CONSTRAINT [FK_MerchandisingSpecificationSheetDocument_MerchandisingSpecificationSheet] FOREIGN KEY ([SpecificationSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId]) ON DELETE CASCADE
);

