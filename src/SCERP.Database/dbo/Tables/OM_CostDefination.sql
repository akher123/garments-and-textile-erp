CREATE TABLE [dbo].[OM_CostDefination] (
    [CostDefinationId] INT          IDENTITY (1, 1) NOT NULL,
    [CompId]           VARCHAR (3)  NOT NULL,
    [CostRefId]        VARCHAR (4)  NOT NULL,
    [CostName]         VARCHAR (50) NOT NULL,
    [CostGroup]        VARCHAR (3)  NOT NULL,
    CONSTRAINT [PK_OM_CostDefination] PRIMARY KEY CLUSTERED ([CostDefinationId] ASC)
);

