CREATE TABLE [dbo].[OM_Size] (
    [SizeId]    INT          IDENTITY (1, 1) NOT NULL,
    [CompId]    VARCHAR (3)  NULL,
    [SizeRefId] VARCHAR (4)  NULL,
    [SizeName]  VARCHAR (50) NOT NULL,
    [TypeId]    VARCHAR (2)  NULL,
    [ItemType]  VARCHAR (15) NULL,
    CONSTRAINT [PK_OM_Size] PRIMARY KEY CLUSTERED ([SizeId] ASC)
);

