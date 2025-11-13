CREATE TABLE [dbo].[OM_BulkBookingDetail] (
    [BulkBookingDetailId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [BulkBookingId]       BIGINT        NOT NULL,
    [SequenceNo]          INT           NOT NULL,
    [BuyerRefId]          VARCHAR (4)   NOT NULL,
    [OrderNo]             VARCHAR (150) NULL,
    [StyleNo]             VARCHAR (50)  NULL,
    [ItemName]            VARCHAR (150) NULL,
    [Fabrication]         VARCHAR (250) NULL,
    [GSM]                 FLOAT (53)    NULL,
    [ShipDate]            DATETIME      NULL,
    [CompId]              VARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_OM_BulBookingDetail] PRIMARY KEY CLUSTERED ([BulkBookingDetailId] ASC),
    CONSTRAINT [FK_OM_BulBookingDetail_OM_BulkBooking] FOREIGN KEY ([BulkBookingId]) REFERENCES [dbo].[OM_BulkBooking] ([BulkBookingId])
);

