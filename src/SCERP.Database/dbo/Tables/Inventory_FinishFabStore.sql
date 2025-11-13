CREATE TABLE [dbo].[Inventory_FinishFabStore] (
    [FinishFabStoreId]  BIGINT           IDENTITY (1, 1) NOT NULL,
    [FinishFabRefId]    VARCHAR (8)      NOT NULL,
    [InvoiceNo]         VARCHAR (50)     NOT NULL,
    [InvoiceDate]       DATETIME         NULL,
    [RcvRegNo]          VARCHAR (50)     NULL,
    [GateEntryNo]       VARCHAR (50)     NULL,
    [GateEntryDate]     DATETIME         NULL,
    [DyeingSpChallanId] BIGINT           NOT NULL,
    [Remarks]           VARCHAR (150)    NULL,
    [CompId]            VARCHAR (3)      NOT NULL,
    [CratedBy]          UNIQUEIDENTIFIER NOT NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Inventory_FinishFabStore] PRIMARY KEY CLUSTERED ([FinishFabStoreId] ASC),
    CONSTRAINT [FK_Inventory_FinishFabStore_PROD_DyeingSpChallan] FOREIGN KEY ([DyeingSpChallanId]) REFERENCES [dbo].[PROD_DyeingSpChallan] ([DyeingSpChallanId])
);

