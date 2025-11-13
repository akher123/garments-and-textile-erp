CREATE TABLE [dbo].[Mrc_PackageInformation] (
    [PackageInfoId] INT              IDENTITY (1, 1) NOT NULL,
    [OrderId]       INT              NOT NULL,
    [PackageName]   NVARCHAR (50)    NULL,
    [SizeId]        INT              NULL,
    [ColorId]       INT              NULL,
    [Quantity]      INT              NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_PackageInfo] PRIMARY KEY CLUSTERED ([PackageInfoId] ASC),
    CONSTRAINT [FK_Mrc_PackageInformation_Mrc_OrderInformation] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Mrc_OrderInformation] ([OrderId]),
    CONSTRAINT [FK_Mrc_PackageInformation_Mrc_StyleColor1] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Mrc_StyleColor] ([StyleColorId]),
    CONSTRAINT [FK_Mrc_PackageInformation_Mrc_StyleSize] FOREIGN KEY ([SizeId]) REFERENCES [dbo].[Mrc_StyleSize] ([StyleSizeId])
);

