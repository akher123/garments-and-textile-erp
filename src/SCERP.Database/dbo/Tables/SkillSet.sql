CREATE TABLE [dbo].[SkillSet] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (100)   NOT NULL,
    [TitleInBengali] NVARCHAR (100)   NOT NULL,
    [Description]    NVARCHAR (MAX)   NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_SkillSet] PRIMARY KEY CLUSTERED ([Id] ASC)
);

