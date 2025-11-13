CREATE TABLE [dbo].[CommExportDetail] (
    [ExportDetailId]  INT              IDENTITY (1, 1) NOT NULL,
    [ExportId]        BIGINT           NOT NULL,
    [OrderStyleRefId] NVARCHAR (7)     NOT NULL,
    [CartonQuantity]  INT              NULL,
    [ItemQuantity]    INT              NULL,
    [Rate]            DECIMAL (18, 2)  NULL,
    [ItemDescription] NVARCHAR (200)   NULL,
    [ExportCode]      NVARCHAR (100)   NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_CommExportDetail] PRIMARY KEY CLUSTERED ([ExportDetailId] ASC),
    CONSTRAINT [FK_CommExportDetail_CommExport] FOREIGN KEY ([ExportId]) REFERENCES [dbo].[CommExport] ([ExportId])
);

