CREATE TABLE [dbo].[BloodGroup] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [GroupName]   NVARCHAR (100)   NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [CDT]         DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EDT]         DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              NOT NULL,
    CONSTRAINT [PK_BloodGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

