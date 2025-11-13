CREATE TABLE [dbo].[TmModule] (
    [ModuleId]   INT           IDENTITY (1, 1) NOT NULL,
    [ModuleName] NVARCHAR (50) NOT NULL,
    [CompId]     NVARCHAR (3)  NULL,
    CONSTRAINT [PK_TMModule] PRIMARY KEY CLUSTERED ([ModuleId] ASC)
);

