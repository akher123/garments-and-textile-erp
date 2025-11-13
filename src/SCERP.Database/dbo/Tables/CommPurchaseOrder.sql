CREATE TABLE [dbo].[CommPurchaseOrder] (
    [PurchaseOrderId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [CompId]             VARCHAR (3)      NOT NULL,
    [PurchaseOrderRefId] VARCHAR (10)     NOT NULL,
    [PurchaseOrderNo]    VARCHAR (100)    NOT NULL,
    [PurchaseOrderDate]  DATE             NULL,
    [ExpDate]            DATE             NULL,
    [SupplierId]         INT              NOT NULL,
    [OrderNo]            VARCHAR (12)     NOT NULL,
    [OrderStyleRefId]    VARCHAR (7)      NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NULL,
    [PType]              CHAR (1)         NOT NULL,
    [Rmks]               NVARCHAR (MAX)   NULL,
    [IsApproved]         BIT              NULL,
    [ApprovedBy]         UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_CM_PurchaseOrder] PRIMARY KEY CLUSTERED ([PurchaseOrderId] ASC)
);

