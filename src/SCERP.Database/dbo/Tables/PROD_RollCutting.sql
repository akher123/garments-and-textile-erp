CREATE TABLE [dbo].[PROD_RollCutting] (
    [RollCuttingId]     BIGINT       IDENTITY (1, 1) NOT NULL,
    [CompId]            VARCHAR (3)  NOT NULL,
    [CuttingBatchId]    BIGINT       NOT NULL,
    [CuttingBatchRefId] VARCHAR (6)  NULL,
    [RollNo]            VARCHAR (2)  NULL,
    [ColorRefId]        VARCHAR (4)  NULL,
    [BatchNo]           VARCHAR (12) NULL,
    [Quantity]          INT          NULL,
    [RollSart]          INT          NULL,
    [RollEnd]           INT          NULL,
    [Combo]             VARCHAR (15) NULL,
    [RollName]          VARCHAR (15) NULL,
    CONSTRAINT [PK_PROD_RollCutting] PRIMARY KEY CLUSTERED ([RollCuttingId] ASC),
    CONSTRAINT [FK_PROD_RollCutting_PROD_CuttingBatch] FOREIGN KEY ([CuttingBatchId]) REFERENCES [dbo].[PROD_CuttingBatch] ([CuttingBatchId])
);

