CREATE TABLE [dbo].[UserRole] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [UserName]        NVARCHAR (100)   NOT NULL,
    [ModuleFeatureId] INT              NOT NULL,
    [AccessLevel]     INT              NULL,
    [CDT]             DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EDT]             DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NULL,
    [CompId]          CHAR (3)         NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserRole_ModuleFeature] FOREIGN KEY ([ModuleFeatureId]) REFERENCES [dbo].[ModuleFeature] ([Id]) ON DELETE CASCADE
);

