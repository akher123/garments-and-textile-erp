CREATE TABLE [dbo].[PROD_LayCutting] (
    [LayCuttingId]      BIGINT      IDENTITY (1, 1) NOT NULL,
    [CompId]            VARCHAR (3) NULL,
    [CuttingBatchId]    BIGINT      NOT NULL,
    [CuttingBatchRefId] VARCHAR (6) NULL,
    [LaySl]             VARCHAR (2) NULL,
    [SizeRefId]         VARCHAR (4) NULL,
    [Ratio]             INT         NULL,
    CONSTRAINT [PK_PROD_LayCutting] PRIMARY KEY CLUSTERED ([LayCuttingId] ASC),
    CONSTRAINT [FK_PROD_LayCutting_PROD_CuttingBatch] FOREIGN KEY ([CuttingBatchId]) REFERENCES [dbo].[PROD_CuttingBatch] ([CuttingBatchId])
);

