CREATE TABLE [dbo].[Inventory_Booking] (
    [BookingId]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompId]         VARCHAR (3)    NOT NULL,
    [BookingRefId]   CHAR (8)       NOT NULL,
    [PiNo]           CHAR (15)      NULL,
    [BookingDate]    DATE           NOT NULL,
    [SupplierId]     INT            NOT NULL,
    [OrderNo]        VARCHAR (150)  NOT NULL,
    [StyleNo]        VARCHAR (150)  NOT NULL,
    [OrderQty]       INT            NOT NULL,
    [MarchandiserId] INT            NOT NULL,
    [BuyerId]        BIGINT         NOT NULL,
    [StoreId]        INT            NOT NULL,
    [Remarks]        NVARCHAR (150) NULL,
    CONSTRAINT [PK_Inventory_Booking] PRIMARY KEY CLUSTERED ([BookingId] ASC),
    CONSTRAINT [FK_Inventory_Booking_Mrc_SupplierCompany] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Mrc_SupplierCompany] ([SupplierCompanyId]),
    CONSTRAINT [FK_Inventory_Booking_OM_Buyer] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[OM_Buyer] ([BuyerId]),
    CONSTRAINT [FK_Inventory_Booking_OM_Merchandiser] FOREIGN KEY ([MarchandiserId]) REFERENCES [dbo].[OM_Merchandiser] ([MerchandiserId])
);

