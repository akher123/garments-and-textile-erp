CREATE TABLE [dbo].[PROD_ProcessReceiveDetail] (
    [ProcessReceiveDetailId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [ProcessReceiveId]       BIGINT      NOT NULL,
    [CuttingBatchId]         BIGINT      NOT NULL,
    [ColorRefId]             VARCHAR (4) NULL,
    [SizeRefId]              VARCHAR (4) NULL,
    [CuttingTagId]           BIGINT      NOT NULL,
    [ReceivedQty]            INT         NOT NULL,
    [CompId]                 VARCHAR (3) NOT NULL,
    [InvocieQty]             INT         NOT NULL,
    [FabricReject]           INT         NOT NULL,
    [ProcessReject]          INT         NOT NULL,
    CONSTRAINT [PK_PROD_ProcessReceiveDetail] PRIMARY KEY CLUSTERED ([ProcessReceiveDetailId] ASC),
    CONSTRAINT [FK_PROD_ProcessReceiveDetail_PROD_CuttingBatch] FOREIGN KEY ([CuttingBatchId]) REFERENCES [dbo].[PROD_CuttingBatch] ([CuttingBatchId]),
    CONSTRAINT [FK_PROD_ProcessReceiveDetail_PROD_CuttingTag] FOREIGN KEY ([CuttingTagId]) REFERENCES [dbo].[PROD_CuttingTag] ([CuttingTagId]),
    CONSTRAINT [FK_PROD_ProcessReceiveDetail_PROD_ProcessReceive] FOREIGN KEY ([ProcessReceiveId]) REFERENCES [dbo].[PROD_ProcessReceive] ([ProcessReceiveId])
);

