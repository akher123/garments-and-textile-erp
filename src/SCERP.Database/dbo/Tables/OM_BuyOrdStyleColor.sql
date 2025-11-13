CREATE TABLE [dbo].[OM_BuyOrdStyleColor] (
    [OrderStyleColorId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [CompId]            VARCHAR (3) NULL,
    [OrderStyleRefId]   VARCHAR (7) NOT NULL,
    [ColorRefId]        VARCHAR (4) NULL,
    [ColorRow]          INT         NULL,
    CONSTRAINT [PK_OM_BuyOrdStyleColor] PRIMARY KEY CLUSTERED ([OrderStyleColorId] ASC)
);

