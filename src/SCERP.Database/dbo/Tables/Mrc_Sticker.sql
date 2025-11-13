CREATE TABLE [dbo].[Mrc_Sticker] (
    [OrderStickerId]       INT              IDENTITY (1, 1) NOT NULL,
    [PackageId]            INT              NOT NULL,
    [SizeId]               INT              NULL,
    [ColorId]              INT              NULL,
    [BarCode]              NVARCHAR (100)   NULL,
    [Variant]              NVARCHAR (100)   NULL,
    [PlaceHolderofBarCode] NVARCHAR (100)   NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_Sticker] PRIMARY KEY CLUSTERED ([OrderStickerId] ASC),
    CONSTRAINT [FK_Mrc_Sticker_Mrc_PackageInformation] FOREIGN KEY ([PackageId]) REFERENCES [dbo].[Mrc_PackageInformation] ([PackageInfoId]),
    CONSTRAINT [FK_Mrc_Sticker_Mrc_StyleColor] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Mrc_StyleColor] ([StyleColorId]),
    CONSTRAINT [FK_Mrc_Sticker_Mrc_StyleSize] FOREIGN KEY ([SizeId]) REFERENCES [dbo].[Mrc_StyleSize] ([StyleSizeId])
);

