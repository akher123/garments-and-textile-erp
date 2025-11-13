CREATE TABLE [dbo].[OM_OrderType] (
    [OrderTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [OTypeRefId]  NVARCHAR (2)  NOT NULL,
    [OTypeName]   NVARCHAR (50) NOT NULL,
    [Prefix]      VARCHAR (5)   NULL,
    CONSTRAINT [PK_OM_OrderType] PRIMARY KEY CLUSTERED ([OrderTypeId] ASC)
);

