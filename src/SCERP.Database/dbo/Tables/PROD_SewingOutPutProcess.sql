CREATE TABLE [dbo].[PROD_SewingOutPutProcess] (
    [SewingOutPutProcessId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [SewingOutPutProcessRefId] VARCHAR (12)     NOT NULL,
    [LineId]                   INT              NOT NULL,
    [BuyerRefId]               VARCHAR (3)      NOT NULL,
    [OrderNo]                  VARCHAR (12)     NOT NULL,
    [OrderStyleRefId]          VARCHAR (7)      NOT NULL,
    [ColorRefId]               VARCHAR (4)      NOT NULL,
    [PreparedBy]               UNIQUEIDENTIFIER NOT NULL,
    [CompId]                   VARCHAR (3)      NOT NULL,
    [HourId]                   INT              NOT NULL,
    [Remarks]                  VARCHAR (200)    NULL,
    [OutputDate]               DATETIME         NOT NULL,
    [ManPower]                 INT              NOT NULL,
    [BatchNo]                  VARCHAR (12)     NULL,
    [JobNo]                    VARCHAR (50)     NULL,
    [OrderShipRefId]           VARCHAR (8)      NULL,
    CONSTRAINT [PK_PROD_SewingOutPutProcess] PRIMARY KEY CLUSTERED ([SewingOutPutProcessId] ASC),
    CONSTRAINT [FK_PROD_SewingOutPutProcess_PROD_Hour] FOREIGN KEY ([HourId]) REFERENCES [dbo].[PROD_Hour] ([HourId])
);

