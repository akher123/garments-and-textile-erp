CREATE TABLE [dbo].[OM_BulkBooking] (
    [BulkBookingId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [BulkBookingRefId] VARCHAR (8)    NOT NULL,
    [BookingDate]      DATETIME       NULL,
    [Attention]        VARCHAR (150)  NULL,
    [MerchadiserId]    INT            NOT NULL,
    [Note]             NVARCHAR (MAX) NULL,
    [CompId]           VARCHAR (3)    NOT NULL,
    CONSTRAINT [PK_OM_BulkBooking] PRIMARY KEY CLUSTERED ([BulkBookingId] ASC),
    CONSTRAINT [FK_OM_BulkBooking_OM_Merchandiser] FOREIGN KEY ([MerchadiserId]) REFERENCES [dbo].[OM_Merchandiser] ([MerchandiserId])
);

