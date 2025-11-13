CREATE TABLE [dbo].[Mrc_SpecificationSheetImage] (
    [ImageId]              INT              IDENTITY (1, 1) NOT NULL,
    [Title]                NVARCHAR (100)   NULL,
    [Path]                 NVARCHAR (100)   NOT NULL,
    [SpecificationSheetId] INT              NOT NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_MerchandisingSpecificationSheetImage] PRIMARY KEY CLUSTERED ([ImageId] ASC),
    CONSTRAINT [FK_MerchandisingSpecificationSheetImage_MerchandisingSpecificationSheet] FOREIGN KEY ([SpecificationSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId]) ON DELETE CASCADE
);

