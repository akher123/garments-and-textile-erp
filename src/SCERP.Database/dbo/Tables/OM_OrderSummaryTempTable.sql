CREATE TABLE [dbo].[OM_OrderSummaryTempTable] (
    [OrderStyleRefId] VARCHAR (7)     NULL,
    [Merchant]        VARCHAR (100)   NULL,
    [Buyer]           VARCHAR (100)   NULL,
    [OrderNo]         VARCHAR (100)   NULL,
    [StyleName]       VARCHAR (100)   NULL,
    [Qty]             INT             NULL,
    [Fob]             NUMERIC (18, 3) NULL,
    [Month1]          INT             NULL,
    [Month2]          INT             NULL,
    [Month3]          INT             NULL,
    [Month4]          INT             NULL,
    [Month5]          INT             NULL,
    [Month6]          INT             NULL,
    [Month7]          INT             NULL,
    [Amount]          NUMERIC (18, 3) NULL,
    [ShipDate]        DATETIME        NULL,
    [CompId]          VARCHAR (3)     NULL
);

