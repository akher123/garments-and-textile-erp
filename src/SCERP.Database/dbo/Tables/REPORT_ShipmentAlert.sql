CREATE TABLE [dbo].[REPORT_ShipmentAlert] (
    [BuyerRefId]      VARCHAR (4)  NULL,
    [MerchandiserId]  INT          NULL,
    [OrderNo]         VARCHAR (12) NULL,
    [OrderStyleRefId] VARCHAR (7)  NULL,
    [Quantity]        INT          NULL,
    [EFD]             DATETIME     NULL,
    [CuttingQty]      INT          NULL,
    [SewingQty]       INT          NULL,
    [FinshQty]        INT          NULL,
    [ShipedQty]       INT          NULL,
    [ShipedDate]      DATETIME     NULL,
    [XStatus]         VARCHAR (50) NULL,
    [ColorCode]       VARCHAR (50) NULL
);

