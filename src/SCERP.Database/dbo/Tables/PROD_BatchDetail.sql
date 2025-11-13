CREATE TABLE [dbo].[PROD_BatchDetail] (
    [BatchDetailId]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [CompId]         VARCHAR (3)   NOT NULL,
    [BatchId]        BIGINT        NOT NULL,
    [ItemId]         INT           NOT NULL,
    [FdiaSizeRefId]  VARCHAR (4)   NOT NULL,
    [MdiaSizeRefId]  VARCHAR (4)   NOT NULL,
    [ComponentRefId] VARCHAR (3)   NOT NULL,
    [GSM]            VARCHAR (50)  NOT NULL,
    [Quantity]       FLOAT (53)    NOT NULL,
    [Remarks]        VARCHAR (150) NULL,
    [Rate]           FLOAT (53)    NULL,
    [StLength]       FLOAT (53)    NULL,
    [FLength]        FLOAT (53)    NULL,
    [RollQty]        FLOAT (53)    NULL,
    CONSTRAINT [PK_PROD_BatchDetail] PRIMARY KEY CLUSTERED ([BatchDetailId] ASC),
    CONSTRAINT [FK_PROD_BatchDetail_Pro_Batch] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[Pro_Batch] ([BatchId])
);

