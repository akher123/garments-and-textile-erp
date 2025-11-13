CREATE TABLE [dbo].[OM_FabConsumption] (
    [FabConsumptionId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [ConsRefId]        VARCHAR (10)    NULL,
    [ConsDate]         DATETIME        NULL,
    [StyleIRefId]      VARCHAR (4)     NULL,
    [ConsGroup]        VARCHAR (1)     NULL,
    [ConsTypeRefId]    VARCHAR (1)     NULL,
    [TotalQty]         DECIMAL (18, 2) NULL,
    [OrderNo]          VARCHAR (12)    NULL,
    [OrderStyleRefId]  VARCHAR (7)     NULL,
    [CompId]           VARCHAR (3)     NULL,
    CONSTRAINT [PK_OM_FabConsumption] PRIMARY KEY CLUSTERED ([FabConsumptionId] ASC)
);

