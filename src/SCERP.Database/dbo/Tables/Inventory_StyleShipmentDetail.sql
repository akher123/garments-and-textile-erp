CREATE TABLE [dbo].[Inventory_StyleShipmentDetail] (
    [StyleShipmentDetailId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [CompId]                VARCHAR (3) NOT NULL,
    [StyleShipmentId]       BIGINT      NOT NULL,
    [OrderStyleRefId]       VARCHAR (7) NOT NULL,
    [ColorRefId]            VARCHAR (4) NOT NULL,
    [SizeRefId]             VARCHAR (4) NOT NULL,
    [ShipmentQty]           INT         NOT NULL,
    CONSTRAINT [PK_Inventory_StyleShipmentDetail] PRIMARY KEY CLUSTERED ([StyleShipmentDetailId] ASC),
    CONSTRAINT [FK_Inventory_StyleShipmentDetail_Inventory_StyleShipment] FOREIGN KEY ([StyleShipmentId]) REFERENCES [dbo].[Inventory_StyleShipment] ([StyleShipmentId])
);

