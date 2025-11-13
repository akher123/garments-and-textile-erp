CREATE TABLE [dbo].[PROD_PartCutting] (
    [PartCuttingId]     INT          IDENTITY (1, 1) NOT NULL,
    [CompId]            NVARCHAR (3) NOT NULL,
    [CuttingBatchId]    BIGINT       NOT NULL,
    [CuttingBatchRefId] VARCHAR (6)  NULL,
    [PartSL]            VARCHAR (2)  NOT NULL,
    [ComponentRefId]    NVARCHAR (3) NULL,
    CONSTRAINT [PK_PROD_PartCutting] PRIMARY KEY CLUSTERED ([PartCuttingId] ASC),
    CONSTRAINT [FK_PROD_PartCutting_PROD_CuttingBatch] FOREIGN KEY ([CuttingBatchId]) REFERENCES [dbo].[PROD_CuttingBatch] ([CuttingBatchId])
);

