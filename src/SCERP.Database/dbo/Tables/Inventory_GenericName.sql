CREATE TABLE [dbo].[Inventory_GenericName] (
    [GenericNameId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [Description]   VARCHAR (150) NULL,
    [CompId]        VARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_Inventory_Generic] PRIMARY KEY CLUSTERED ([GenericNameId] ASC)
);

