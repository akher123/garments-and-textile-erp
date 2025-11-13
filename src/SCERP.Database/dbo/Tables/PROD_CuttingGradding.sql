CREATE TABLE [dbo].[PROD_CuttingGradding] (
    [CuttingGraddingId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [CuttingBatchId]    BIGINT      NOT NULL,
    [FromSizeRefId]     VARCHAR (4) NOT NULL,
    [ToSizeRefId]       VARCHAR (4) NOT NULL,
    [Quantity]          INT         NOT NULL,
    [CompId]            VARCHAR (3) NOT NULL,
    CONSTRAINT [PK_PROD_CuttingGradding] PRIMARY KEY CLUSTERED ([CuttingGraddingId] ASC),
    CONSTRAINT [FK_PROD_CuttingGradding_PROD_CuttingBatch] FOREIGN KEY ([CuttingBatchId]) REFERENCES [dbo].[PROD_CuttingBatch] ([CuttingBatchId])
);

