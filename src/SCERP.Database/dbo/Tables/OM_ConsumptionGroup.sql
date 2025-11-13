CREATE TABLE [dbo].[OM_ConsumptionGroup] (
    [ConsGroupId]   INT          IDENTITY (1, 1) NOT NULL,
    [ConsGroup]     VARCHAR (1)  NULL,
    [ConsGroupName] VARCHAR (50) NULL,
    CONSTRAINT [PK_OM_ConsumptionGroup] PRIMARY KEY CLUSTERED ([ConsGroupId] ASC)
);

