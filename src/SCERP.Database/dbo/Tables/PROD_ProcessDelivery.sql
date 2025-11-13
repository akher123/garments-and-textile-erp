CREATE TABLE [dbo].[PROD_ProcessDelivery] (
    [ProcessDeliveryId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [RefNo]             VARCHAR (8)      NOT NULL,
    [PartyId]           BIGINT           NOT NULL,
    [BuyerRefId]        VARCHAR (3)      NOT NULL,
    [OrderNo]           VARCHAR (12)     NOT NULL,
    [OrderStyleRefId]   VARCHAR (7)      NOT NULL,
    [ProcessRefId]      VARCHAR (3)      NOT NULL,
    [InvoiceNo]         VARCHAR (10)     NOT NULL,
    [InvDate]           DATE             NOT NULL,
    [CompId]            VARCHAR (3)      NULL,
    [PreparedBy]        UNIQUEIDENTIFIER NOT NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [Remarks]           NVARCHAR (150)   NULL,
    CONSTRAINT [PK_PROD_ProcessDelivery] PRIMARY KEY CLUSTERED ([ProcessDeliveryId] ASC),
    CONSTRAINT [FK_PROD_ProcessDelivery_Party] FOREIGN KEY ([PartyId]) REFERENCES [dbo].[Party] ([PartyId])
);

