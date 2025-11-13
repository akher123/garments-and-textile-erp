CREATE TABLE [dbo].[CommBbLcPurchase] (
    [BbLcPurchaseId]     INT              IDENTITY (1, 1) NOT NULL,
    [BbLcRefId]          INT              NULL,
    [BbLcNo]             NVARCHAR (50)    NULL,
    [PurchaseOrderRefId] BIGINT           NULL,
    [PurchaseOrderNo]    NVARCHAR (15)    NULL,
    [CompId]             NVARCHAR (3)     NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              NOT NULL,
    CONSTRAINT [PK_CommBbLcPurchase] PRIMARY KEY CLUSTERED ([BbLcPurchaseId] ASC)
);

