CREATE TABLE [dbo].[OM_BuyOrdShipDetail] (
    [OrderShipDetailId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompId]            VARCHAR (3)     NULL,
    [OrderShipRefId]    VARCHAR (8)     NULL,
    [ColorRefId]        VARCHAR (4)     NULL,
    [SizeRefId]         VARCHAR (50)    NULL,
    [Quantity]          INT             NULL,
    [QuantityP]         INT             NULL,
    [PAllow]            NUMERIC (18, 2) NULL,
    [ColorRow]          INT             NULL,
    [SizeRow]           INT             NULL,
    [ShQty]             INT             NULL,
    CONSTRAINT [PK_OM_BuyOrdShipDetail] PRIMARY KEY CLUSTERED ([OrderShipDetailId] ASC)
);

