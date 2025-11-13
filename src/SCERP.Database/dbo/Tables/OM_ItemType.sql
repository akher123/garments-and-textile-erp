CREATE TABLE [dbo].[OM_ItemType] (
    [ItemTypeId]  INT           IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (100) NOT NULL,
    [Description] VARCHAR (100) NOT NULL,
    [CompId]      VARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_OM_ItemType] PRIMARY KEY CLUSTERED ([ItemTypeId] ASC)
);

