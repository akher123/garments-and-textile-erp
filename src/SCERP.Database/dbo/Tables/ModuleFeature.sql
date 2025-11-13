CREATE TABLE [dbo].[ModuleFeature] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [FeatureName]     NVARCHAR (50)    NOT NULL,
    [ParentFeatureId] INT              NULL,
    [ModuleId]        INT              NOT NULL,
    [NavURL]          VARCHAR (200)    NULL,
    [ShowInMenu]      BIT              NULL,
    [OrderId]         INT              NULL,
    [CDT]             DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EDT]             DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NULL,
    CONSTRAINT [PK_ModuleFeature] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ModuleFeature_Module] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[Module] ([Id]) ON DELETE CASCADE
);

