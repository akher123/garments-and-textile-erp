CREATE TABLE [dbo].[PROD_CuttingBatch] (
    [CuttingBatchId]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [CompId]            VARCHAR (3)   NULL,
    [CuttingBatchRefId] VARCHAR (6)   NULL,
    [CuttingDate]       DATE          NULL,
    [BuyerRefId]        VARCHAR (3)   NULL,
    [JobNo]             VARCHAR (50)  NULL,
    [StyleRefId]        VARCHAR (8)   NULL,
    [FIT]               VARCHAR (50)  NULL,
    [OrderNo]           VARCHAR (12)  NULL,
    [Rmks]              VARCHAR (100) NULL,
    [CuttingStatus]     CHAR (1)      NULL,
    [OrderStyleRefId]   VARCHAR (7)   NULL,
    [ColorRefId]        VARCHAR (4)   NULL,
    [ComponentRefId]    VARCHAR (3)   NULL,
    [ApprovalStatus]    CHAR (1)      NULL,
    [MachineId]         INT           NULL,
    [ConsPerDzn]        FLOAT (53)    NULL,
    [MarkerEffPct]      FLOAT (53)    NULL,
    CONSTRAINT [PK_PROD_CuttingBatch] PRIMARY KEY CLUSTERED ([CuttingBatchId] ASC)
);

