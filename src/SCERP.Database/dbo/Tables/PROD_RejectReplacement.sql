CREATE TABLE [dbo].[PROD_RejectReplacement] (
    [RejectReplacementId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [CuttingBatchId]      BIGINT      NOT NULL,
    [RejectQty]           INT         NOT NULL,
    [SizeRefId]           VARCHAR (4) NOT NULL,
    [CompId]              VARCHAR (3) NOT NULL,
    CONSTRAINT [PK_PROD_RejectReplacement] PRIMARY KEY CLUSTERED ([RejectReplacementId] ASC),
    CONSTRAINT [FK_PROD_RejectReplacement_PROD_CuttingBatch] FOREIGN KEY ([CuttingBatchId]) REFERENCES [dbo].[PROD_CuttingBatch] ([CuttingBatchId])
);

