CREATE TABLE [dbo].[Module] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [ModuleName]  NVARCHAR (100)   NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [CDT]         DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EDT]         DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              NULL,
    CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED ([Id] ASC)
);

