CREATE TABLE [dbo].[PROD_SewingInputProcess] (
    [SewingInputProcessId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [SewingInputProcessRefId] VARCHAR (10)     NOT NULL,
    [BuyerRefId]              VARCHAR (3)      NOT NULL,
    [OrderNo]                 VARCHAR (12)     NOT NULL,
    [OrderStyleRefId]         VARCHAR (7)      NOT NULL,
    [ColorRefId]              VARCHAR (4)      NOT NULL,
    [LineId]                  INT              NOT NULL,
    [InputDate]               DATETIME         NULL,
    [CompId]                  VARCHAR (3)      NOT NULL,
    [PreparedBy]              UNIQUEIDENTIFIER NOT NULL,
    [Remarks]                 VARCHAR (200)    NULL,
    [HourId]                  INT              NULL,
    [BatchNo]                 VARCHAR (12)     NULL,
    [JobNo]                   VARCHAR (50)     NULL,
    [OrderShipRefId]          VARCHAR (8)      NULL,
    [Locked]                  BIT              NULL,
    [LockedBy]                UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_SewingInputProcess] PRIMARY KEY CLUSTERED ([SewingInputProcessId] ASC),
    CONSTRAINT [FK_PROD_SewingInputProcess_Production_Machine] FOREIGN KEY ([LineId]) REFERENCES [dbo].[Production_Machine] ([MachineId])
);

