CREATE TABLE [dbo].[OM_BuyOrdStyleSize] (
    [OrderStyleSizeId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [CompId]           VARCHAR (3) NULL,
    [OrderStyleRefId]  VARCHAR (7) NOT NULL,
    [SizeRefId]        VARCHAR (4) NOT NULL,
    [SizeRow]          INT         NULL,
    CONSTRAINT [PK_OM_BuyOrdStyleSize] PRIMARY KEY CLUSTERED ([OrderStyleSizeId] ASC)
);

