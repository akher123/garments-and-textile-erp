CREATE TABLE [dbo].[PROD_FinishingProcess] (
    [FinishingProcessId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [FinishingProcessRefId] VARCHAR (10)     NOT NULL,
    [CompId]                VARCHAR (3)      NOT NULL,
    [InputDate]             DATETIME         NULL,
    [HourId]                INT              NOT NULL,
    [PreparedBy]            UNIQUEIDENTIFIER NULL,
    [FType]                 INT              NOT NULL,
    [BuyerRefId]            VARCHAR (3)      NOT NULL,
    [OrderNo]               VARCHAR (12)     NOT NULL,
    [OrderStyleRefId]       VARCHAR (7)      NOT NULL,
    [ColorRefId]            VARCHAR (4)      NOT NULL,
    [Remarks]               VARCHAR (200)    NULL,
    [OrderShipRefId]        VARCHAR (8)      NULL,
    CONSTRAINT [PK_PROD_FinishingProcess] PRIMARY KEY CLUSTERED ([FinishingProcessId] ASC),
    CONSTRAINT [FK_PROD_FinishingProcess_PROD_Hour] FOREIGN KEY ([HourId]) REFERENCES [dbo].[PROD_Hour] ([HourId])
);

