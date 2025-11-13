CREATE TABLE [dbo].[PROD_RejectAdjustment] (
    [RejectAdjustmentId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [CuttingBatchId]     BIGINT      NOT NULL,
    [RejectQty]          INT         NOT NULL,
    [SizeRefId]          VARCHAR (4) NOT NULL,
    [CompId]             VARCHAR (3) NOT NULL,
    CONSTRAINT [PK_PROD_RejectAdjustment] PRIMARY KEY CLUSTERED ([RejectAdjustmentId] ASC),
    CONSTRAINT [FK_PROD_RejectAdjustment_PROD_CuttingBatch] FOREIGN KEY ([CuttingBatchId]) REFERENCES [dbo].[PROD_CuttingBatch] ([CuttingBatchId])
);

