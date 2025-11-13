CREATE TABLE [dbo].[OM_ConsumptionType] (
    [ConsTypeId]    INT          IDENTITY (1, 1) NOT NULL,
    [ConsTypeRefId] VARCHAR (1)  NULL,
    [ConsTypeName]  VARCHAR (50) NULL,
    CONSTRAINT [PK_OM_ConsumptionType] PRIMARY KEY CLUSTERED ([ConsTypeId] ASC)
);

