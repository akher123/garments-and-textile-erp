CREATE TABLE [dbo].[CommBbLcPurchaseCommon] (
    [BbLcPurchaseId]     INT              IDENTITY (1, 1) NOT NULL,
    [BbLcRefId]          INT              NULL,
    [BbLcNo]             NVARCHAR (50)    NULL,
    [PurchaseOrderRefId] BIGINT           NULL,
    [PurchaseOrderNo]    NVARCHAR (15)    NULL,
    [PurchaseDate]       DATETIME         NULL,
    [ItemCode]           NVARCHAR (8)     NULL,
    [ColorRefId]         NVARCHAR (4)     NULL,
    [SizeRefId]          NVARCHAR (4)     NULL,
    [Quantity]           DECIMAL (18, 5)  NULL,
    [xRate]              DECIMAL (18, 5)  NULL,
    [PurchaseType]       NVARCHAR (1)     NULL,
    [CompId]             NVARCHAR (3)     NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              NOT NULL,
    CONSTRAINT [PK_CommBbLcPurchaseCommon] PRIMARY KEY CLUSTERED ([BbLcPurchaseId] ASC)
);

