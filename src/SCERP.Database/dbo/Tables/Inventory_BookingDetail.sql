CREATE TABLE [dbo].[Inventory_BookingDetail] (
    [BookingDetailId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [BookingId]       BIGINT          NOT NULL,
    [CompId]          VARCHAR (3)     NOT NULL,
    [ColorRefId]      VARCHAR (4)     NULL,
    [SizeRefId]       VARCHAR (4)     NULL,
    [ItemId]          INT             NOT NULL,
    [Quantity]        DECIMAL (18, 5) NOT NULL,
    [Rate]            DECIMAL (18, 5) NOT NULL,
    [FColorRefId]     VARCHAR (4)     NULL,
    CONSTRAINT [PK_Inventory_BookingDetail] PRIMARY KEY CLUSTERED ([BookingDetailId] ASC),
    CONSTRAINT [FK_Inventory_BookingDetail_Inventory_Booking] FOREIGN KEY ([BookingId]) REFERENCES [dbo].[Inventory_Booking] ([BookingId])
);

