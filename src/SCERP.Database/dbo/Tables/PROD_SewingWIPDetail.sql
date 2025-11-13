CREATE TABLE [dbo].[PROD_SewingWIPDetail] (
    [LineId]          INT          NOT NULL,
    [BuyerRefId]      VARCHAR (3)  NOT NULL,
    [OrderNo]         VARCHAR (12) NOT NULL,
    [OrderStyleRefId] VARCHAR (7)  NOT NULL,
    [ColorRefId]      VARCHAR (4)  NOT NULL,
    [IQty]            INT          NOT NULL,
    [OQty]            INT          NOT NULL
);

