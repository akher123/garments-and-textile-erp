CREATE TABLE [dbo].[OM_CostGroup] (
    [CostGroupId] INT          IDENTITY (1, 1) NOT NULL,
    [GroupCode]   CHAR (3)     NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    [CompId]      VARCHAR (3)  NOT NULL,
    CONSTRAINT [PK_OM_CostGroup] PRIMARY KEY CLUSTERED ([CostGroupId] ASC)
);

