CREATE TABLE [dbo].[OM_FabricType] (
    [FabricTypeId] INT       NOT NULL,
    [FabricType]   CHAR (1)  NULL,
    [Description]  CHAR (10) NULL,
    CONSTRAINT [PK_OM_FabricType] PRIMARY KEY CLUSTERED ([FabricTypeId] ASC)
);

