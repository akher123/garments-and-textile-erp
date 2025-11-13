CREATE TABLE [dbo].[Inventory_RStock] (
    [ItemId]      INT             NULL,
    [ItemCode]    NVARCHAR (100)  NULL,
    [OQty]        NUMERIC (18, 5) NULL,
    [OAmt]        NUMERIC (18, 5) NULL,
    [RQty]        NUMERIC (18, 5) NULL,
    [RAmt]        NUMERIC (18, 5) NULL,
    [IQty]        NUMERIC (18, 5) NULL,
    [IAmt]        NUMERIC (18, 5) NULL,
    [OutConsQty]  NUMERIC (18, 5) NULL,
    [OutAmt]      NUMERIC (18, 5) NULL,
    [ColorRefId]  VARCHAR (4)     NULL,
    [SizeRefId]   VARCHAR (4)     NULL,
    [BrandId]     INT             NULL,
    [FColorRefId] VARCHAR (4)     NULL,
    [BuyerRefId]  VARCHAR (4)     NULL,
    [CompId]      VARCHAR (3)     NULL
);

