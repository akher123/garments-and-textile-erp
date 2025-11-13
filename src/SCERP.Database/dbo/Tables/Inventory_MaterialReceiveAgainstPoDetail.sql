CREATE TABLE [dbo].[Inventory_MaterialReceiveAgainstPoDetail] (
    [MaterialReceiveAgstPoDetailId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [MaterialReceiveAgstPoId]       BIGINT          NOT NULL,
    [CompId]                        VARCHAR (3)     NOT NULL,
    [ItemId]                        INT             NOT NULL,
    [ColorRefId]                    VARCHAR (4)     NOT NULL,
    [SizeRefId]                     VARCHAR (4)     NOT NULL,
    [ReceivedQty]                   DECIMAL (18, 5) NOT NULL,
    [ReceivedRate]                  DECIMAL (18, 5) NOT NULL,
    [RejectedQty]                   DECIMAL (18, 5) NULL,
    [DiscountQty]                   DECIMAL (18, 5) NULL,
    [FColorRefId]                   VARCHAR (4)     NULL,
    [PurchaseOrderDetailId]         BIGINT          NULL,
    [GSizeRefId]                    VARCHAR (4)     NULL,
    [Location]                      VARCHAR (150)   NULL,
    [OrderStyleRefId]               VARCHAR (7)     NULL,
    CONSTRAINT [PK_Inventory_MaterialReceiveAgainstPoDetail] PRIMARY KEY CLUSTERED ([MaterialReceiveAgstPoDetailId] ASC),
    CONSTRAINT [FK_Inventory_MaterialReceiveAgainstPoDetail_Inventory_MaterialReceiveAgainstPo] FOREIGN KEY ([MaterialReceiveAgstPoId]) REFERENCES [dbo].[Inventory_MaterialReceiveAgainstPo] ([MaterialReceiveAgstPoId])
);

