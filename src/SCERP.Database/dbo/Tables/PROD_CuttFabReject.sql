CREATE TABLE [dbo].[PROD_CuttFabReject] (
    [CuttFabRejectId] INT           IDENTITY (1, 1) NOT NULL,
    [CompId]          VARCHAR (3)   NOT NULL,
    [BatchId]         BIGINT        NOT NULL,
    [BatchDetailId]   BIGINT        NOT NULL,
    [CuttingWit]      FLOAT (53)    NOT NULL,
    [RejectWit]       FLOAT (53)    NOT NULL,
    [ChallanNo]       VARCHAR (50)  NULL,
    [Remarks]         VARCHAR (MAX) NULL,
    [EntryDate]       DATETIME      NOT NULL,
    CONSTRAINT [PK_PROD_CuttFabReject] PRIMARY KEY CLUSTERED ([CuttFabRejectId] ASC)
);

