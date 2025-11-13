CREATE TABLE [dbo].[PROD_ProcessDeliveryDetail] (
    [ProcessDeliveryDetailId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [ProcessDeliveryId]       BIGINT      NOT NULL,
    [CompId]                  VARCHAR (3) NOT NULL,
    [CuttingBatchId]          BIGINT      NOT NULL,
    [ComponentRefId]          VARCHAR (3) NOT NULL,
    [CuttingTagId]            BIGINT      NOT NULL,
    [ColorRefId]              VARCHAR (4) NOT NULL,
    [SizeRefId]               VARCHAR (4) NOT NULL,
    [Quantity]                INT         NOT NULL,
    CONSTRAINT [PK_PROD_ProcessDeliveryDetail] PRIMARY KEY CLUSTERED ([ProcessDeliveryDetailId] ASC),
    CONSTRAINT [FK_PROD_ProcessDeliveryDetail_PROD_ProcessDelivery] FOREIGN KEY ([ProcessDeliveryId]) REFERENCES [dbo].[PROD_ProcessDelivery] ([ProcessDeliveryId])
);

