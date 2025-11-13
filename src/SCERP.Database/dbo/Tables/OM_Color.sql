CREATE TABLE [dbo].[OM_Color] (
    [ColorId]    INT           IDENTITY (1, 1) NOT NULL,
    [CompId]     VARCHAR (3)   NULL,
    [ColorRefId] VARCHAR (4)   NULL,
    [ColorName]  NVARCHAR (50) NOT NULL,
    [ColorCode]  NVARCHAR (50) NULL,
    [TypeId]     VARCHAR (2)   NULL,
    CONSTRAINT [PK_OM_Color] PRIMARY KEY CLUSTERED ([ColorId] ASC)
);

