CREATE TABLE [dbo].[HouseKeepingItem] (
    [HouseKeepingItemId] INT            IDENTITY (1, 1) NOT NULL,
    [HkItemRefId]        VARCHAR (4)    NOT NULL,
    [Name]               NVARCHAR (250) NOT NULL,
    [Description]        NVARCHAR (250) NULL,
    [CompId]             VARCHAR (3)    NOT NULL,
    CONSTRAINT [PK_HouseKeepingItem] PRIMARY KEY CLUSTERED ([HouseKeepingItemId] ASC)
);

