CREATE TABLE [dbo].[Inventory_MaterialReceivedDetail] (
    [MaterialReceivedDetailId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [CompId]                   VARCHAR (3)   NOT NULL,
    [MaterialReceivedId]       BIGINT        NOT NULL,
    [Item]                     VARCHAR (200) NOT NULL,
    [Color]                    VARCHAR (50)  NULL,
    [Size]                     VARCHAR (50)  NULL,
    [Brand]                    VARCHAR (50)  NULL,
    [LotNo]                    VARCHAR (50)  NULL,
    [UnitName]                 VARCHAR (20)  NULL,
    [ReceivedQty]              FLOAT (53)    NULL,
    [Rate]                     FLOAT (53)    NULL,
    [TotalAmount]              FLOAT (53)    NULL,
    [BuyerNameDtl]             VARCHAR (150) NULL,
    [OrderNameDtl]             VARCHAR (150) NULL,
    [StyleNameDtl]             VARCHAR (150) NULL,
    CONSTRAINT [PK_Inventory_MaterialReceivedDetail] PRIMARY KEY CLUSTERED ([MaterialReceivedDetailId] ASC),
    CONSTRAINT [FK_Inventory_MaterialReceivedDetail_Inventory_MaterialReceived] FOREIGN KEY ([MaterialReceivedId]) REFERENCES [dbo].[Inventory_MaterialReceived] ([MaterialReceivedId])
);

