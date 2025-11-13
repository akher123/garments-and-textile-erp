CREATE TABLE [dbo].[OM_Brand] (
    [BrandId]    INT           IDENTITY (1, 1) NOT NULL,
    [CompId]     VARCHAR (3)   NULL,
    [BrandRefId] VARCHAR (3)   NOT NULL,
    [BrandName]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_OM_Brand] PRIMARY KEY CLUSTERED ([BrandId] ASC)
);

