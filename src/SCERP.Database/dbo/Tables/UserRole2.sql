CREATE TABLE [dbo].[UserRole2] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [UserName]        NVARCHAR (100)   NOT NULL,
    [ModuleFeatureId] INT              NOT NULL,
    [AccessLevel]     INT              NULL,
    [CDT]             DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EDT]             DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NULL
);

