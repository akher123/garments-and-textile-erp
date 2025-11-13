CREATE TABLE [dbo].[PROD_FinishingProcessDetail] (
    [FinishingProcessDetailId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [FinishingProcessId]       BIGINT      NOT NULL,
    [SizeRefId]                VARCHAR (4) NOT NULL,
    [InputQuantity]            INT         NOT NULL,
    [CompId]                   VARCHAR (3) NOT NULL,
    CONSTRAINT [PK_PROD_FinishingProcessDetail] PRIMARY KEY CLUSTERED ([FinishingProcessDetailId] ASC),
    CONSTRAINT [FK_PROD_FinishingProcessDetail_PROD_FinishingProcess] FOREIGN KEY ([FinishingProcessId]) REFERENCES [dbo].[PROD_FinishingProcess] ([FinishingProcessId])
);

