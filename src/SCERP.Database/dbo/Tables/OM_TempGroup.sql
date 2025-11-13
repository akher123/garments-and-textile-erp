CREATE TABLE [dbo].[OM_TempGroup] (
    [TempGroupId]   INT           IDENTITY (1, 1) NOT NULL,
    [TempGroupName] VARCHAR (100) NOT NULL,
    [CompId]        VARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_OM_TempGroup] PRIMARY KEY CLUSTERED ([TempGroupId] ASC)
);

