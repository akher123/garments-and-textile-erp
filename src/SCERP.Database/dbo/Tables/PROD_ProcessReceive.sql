CREATE TABLE [dbo].[PROD_ProcessReceive] (
    [ProcessReceiveId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [RefNo]            VARCHAR (8)      NOT NULL,
    [CompId]           VARCHAR (3)      NOT NULL,
    [ProcessRefId]     VARCHAR (3)      NOT NULL,
    [InvoiceNo]        VARCHAR (20)     NOT NULL,
    [InvoiceDate]      DATETIME         NULL,
    [GateEntryNo]      VARCHAR (20)     NULL,
    [GateEntrydate]    DATETIME         NULL,
    [ReceivedBy]       UNIQUEIDENTIFIER NULL,
    [PartyId]          BIGINT           NOT NULL,
    [Remarks]          NVARCHAR (MAX)   NULL,
    [VoucherMasterId]  BIGINT           NULL,
    [Posted]           CHAR (1)         NULL,
    CONSTRAINT [PK_PROD_ProcessReceive] PRIMARY KEY CLUSTERED ([ProcessReceiveId] ASC),
    CONSTRAINT [FK_PROD_ProcessReceive_Party] FOREIGN KEY ([PartyId]) REFERENCES [dbo].[Party] ([PartyId])
);

