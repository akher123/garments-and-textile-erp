CREATE TABLE [dbo].[CommPurchaseOrderDetail] (
    [PurchaseOrderDetailId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [PurchaseOrderId]       BIGINT          NOT NULL,
    [CompId]                VARCHAR (3)     NOT NULL,
    [PurchaseOrderRefId]    VARCHAR (10)    NULL,
    [ItemCode]              VARCHAR (8)     NULL,
    [ColorRefId]            VARCHAR (4)     NULL,
    [SizeRefId]             VARCHAR (4)     NULL,
    [Quantity]              DECIMAL (18, 5) NULL,
    [xRate]                 DECIMAL (18, 5) NULL,
    [GColorRefId]           VARCHAR (4)     NULL,
    [GSizeRefId]            VARCHAR (4)     NULL,
    CONSTRAINT [PK_CM_PurchaseOrderDetail] PRIMARY KEY CLUSTERED ([PurchaseOrderDetailId] ASC),
    CONSTRAINT [FK_CM_PurchaseOrderDetail_CM_PurchaseOrder] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [dbo].[CommPurchaseOrder] ([PurchaseOrderId])
);

