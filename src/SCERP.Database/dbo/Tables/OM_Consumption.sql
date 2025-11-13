CREATE TABLE [dbo].[OM_Consumption] (
    [ConsumptionId]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompId]          VARCHAR (3)     NULL,
    [ConsRefId]       VARCHAR (10)    NULL,
    [ConsDate]        DATE            NULL,
    [OrderStyleRefId] VARCHAR (7)     NULL,
    [ConsGroup]       VARCHAR (1)     NULL,
    [ConsTypeRefId]   VARCHAR (1)     NULL,
    [Quantity]        DECIMAL (18, 3) NULL,
    [ItemCode]        VARCHAR (8)     NULL,
    [SupplierId]      INT             NULL,
    [Rate]            NUMERIC (18, 5) NULL,
    [ItemDescription] VARCHAR (250)   NULL,
    CONSTRAINT [PK_OM_Consumption] PRIMARY KEY CLUSTERED ([ConsumptionId] ASC)
);

