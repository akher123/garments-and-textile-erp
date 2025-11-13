CREATE TABLE [dbo].[Maintenance_ReturnableChallanDetail] (
    [ReturnableChallanDetailId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReturnableChallanId]       BIGINT         NOT NULL,
    [ItemName]                  NVARCHAR (MAX) NOT NULL,
    [Unit]                      VARCHAR (25)   NULL,
    [DeliveryQty]               FLOAT (53)     NOT NULL,
    [ReceiveQty]                FLOAT (53)     NOT NULL,
    [Remarks]                   VARCHAR (MAX)  NULL,
    [CompId]                    VARCHAR (3)    NOT NULL,
    [RejectQty]                 FLOAT (53)     NULL,
    [RollQty]                   INT            NULL,
    [BatchNo]                   VARCHAR (150)  NULL,
    [Buyer]                     VARCHAR (150)  NULL,
    [OrderNo]                   VARCHAR (150)  NULL,
    [StyleNo]                   VARCHAR (150)  NULL,
    [Color]                     VARCHAR (150)  NULL,
    CONSTRAINT [PK_Maintenance_ReturnableChallanDetail] PRIMARY KEY CLUSTERED ([ReturnableChallanDetailId] ASC),
    CONSTRAINT [FK_Maintenance_ReturnableChallanDetail_Maintenance_ReturnableChallan] FOREIGN KEY ([ReturnableChallanId]) REFERENCES [dbo].[Maintenance_ReturnableChallan] ([ReturnableChallanId])
);

