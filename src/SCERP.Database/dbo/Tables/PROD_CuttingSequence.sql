CREATE TABLE [dbo].[PROD_CuttingSequence] (
    [CuttingSequenceId]    BIGINT       IDENTITY (1, 1) NOT NULL,
    [CuttingSequenceRefId] VARCHAR (8)  NOT NULL,
    [CompId]               VARCHAR (3)  NOT NULL,
    [BuyerRefId]           VARCHAR (3)  NOT NULL,
    [OrderNo]              VARCHAR (12) NOT NULL,
    [OrderStyleRefId]      VARCHAR (7)  NOT NULL,
    [ColorRefId]           VARCHAR (4)  NOT NULL,
    [SlNo]                 INT          NOT NULL,
    [ComponentRefId]       VARCHAR (3)  NULL,
    CONSTRAINT [PK_PROD_CuttingProcess] PRIMARY KEY CLUSTERED ([CuttingSequenceId] ASC)
);

