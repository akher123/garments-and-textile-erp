CREATE TABLE [dbo].[OM_ItemMode] (
    [ItemModeId] INT           IDENTITY (1, 1) NOT NULL,
    [IModeRefId] VARCHAR (1)   NULL,
    [IModeName]  NVARCHAR (50) NULL,
    CONSTRAINT [PK_OM_ItemMode] PRIMARY KEY CLUSTERED ([ItemModeId] ASC)
);

