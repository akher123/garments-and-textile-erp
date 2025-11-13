CREATE TABLE [dbo].[OM_CostSheetTemplate] (
    [TemplateId]  INT           IDENTITY (1, 1) NOT NULL,
    [SerialNo]    INT           NOT NULL,
    [Particular]  VARCHAR (150) NOT NULL,
    [TempGroupId] INT           NOT NULL,
    [ItemTypeId]  INT           NOT NULL,
    [UnitName]    VARCHAR (50)  NOT NULL,
    [CompId]      VARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_OM_CostSheetTemplate] PRIMARY KEY CLUSTERED ([TemplateId] ASC),
    CONSTRAINT [FK_OM_CostSheetTemplate_OM_ItemType] FOREIGN KEY ([ItemTypeId]) REFERENCES [dbo].[OM_ItemType] ([ItemTypeId]),
    CONSTRAINT [FK_OM_CostSheetTemplate_OM_TempGroup] FOREIGN KEY ([TempGroupId]) REFERENCES [dbo].[OM_TempGroup] ([TempGroupId])
);

