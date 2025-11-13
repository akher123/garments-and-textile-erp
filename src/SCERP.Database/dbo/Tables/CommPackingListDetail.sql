CREATE TABLE [dbo].[CommPackingListDetail] (
    [PackingListId]   INT              IDENTITY (1, 1) NOT NULL,
    [ExportId]        BIGINT           NULL,
    [Block]           INT              NULL,
    [CountryName]     NVARCHAR (50)    NULL,
    [OrderStyleRefId] NVARCHAR (7)     NULL,
    [ColorName]       NVARCHAR (50)    NULL,
    [SizeName]        NVARCHAR (50)    NULL,
    [CartonQuantity]  INT              NULL,
    [CartonCapacity]  INT              NULL,
    [CartonFrom]      INT              NULL,
    [CartonTo]        INT              NULL,
    [ContainerNo]     NVARCHAR (50)    NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_PackingListDetail] PRIMARY KEY CLUSTERED ([PackingListId] ASC),
    CONSTRAINT [FK_CommPackingListDetail_CommExport] FOREIGN KEY ([ExportId]) REFERENCES [dbo].[CommExport] ([ExportId])
);

