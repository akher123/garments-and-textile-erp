CREATE TABLE [dbo].[OM_EmbWorkOrder] (
    [EmbWorkOrderId]    INT              IDENTITY (1, 1) NOT NULL,
    [RefId]             CHAR (7)         NOT NULL,
    [PartyId]           BIGINT           NOT NULL,
    [MerchandiserRefId] CHAR (4)         NOT NULL,
    [ProcessRefId]      CHAR (3)         NOT NULL,
    [BookingDate]       DATE             NULL,
    [ExpectedDate]      DATE             NULL,
    [Attention]         VARCHAR (100)    NULL,
    [Remarks]           NVARCHAR (MAX)   NULL,
    [IsApproved]        BIT              NOT NULL,
    [ApprovedBy]        UNIQUEIDENTIFIER NULL,
    [ApprovedDate]      DATE             NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]       DATE             NOT NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATE             NULL,
    [CompId]            CHAR (3)         NULL,
    CONSTRAINT [PK_OM_EmbWorkOrder] PRIMARY KEY CLUSTERED ([EmbWorkOrderId] ASC),
    CONSTRAINT [FK_OM_EmbWorkOrder_Party] FOREIGN KEY ([PartyId]) REFERENCES [dbo].[Party] ([PartyId])
);

