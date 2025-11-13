CREATE TABLE [dbo].[Inventory_FinishFabDetailStore] (
    [FinishFabDetailSotreId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [FinishFabStoreId]       BIGINT        NOT NULL,
    [BatchId]                BIGINT        NOT NULL,
    [BatchDetailId]          BIGINT        NOT NULL,
    [RcvQty]                 FLOAT (53)    NOT NULL,
    [RejQty]                 FLOAT (53)    NOT NULL,
    [Remarks]                VARCHAR (150) NULL,
    [CompId]                 VARCHAR (3)   NULL,
    [GreyWt]                 FLOAT (53)    NOT NULL,
    [CcuffQty]               FLOAT (53)    NULL,
    CONSTRAINT [PK_Inventory_FinishFabDetailStore] PRIMARY KEY CLUSTERED ([FinishFabDetailSotreId] ASC),
    CONSTRAINT [FK_Inventory_FinishFabDetailStore_Inventory_FinishFabStore] FOREIGN KEY ([FinishFabStoreId]) REFERENCES [dbo].[Inventory_FinishFabStore] ([FinishFabStoreId]),
    CONSTRAINT [FK_Inventory_FinishFabDetailStore_Pro_Batch] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[Pro_Batch] ([BatchId]),
    CONSTRAINT [FK_Inventory_FinishFabDetailStore_PROD_BatchDetail] FOREIGN KEY ([BatchDetailId]) REFERENCES [dbo].[PROD_BatchDetail] ([BatchDetailId])
);

