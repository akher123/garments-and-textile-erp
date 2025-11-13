CREATE TABLE [dbo].[SkillLevel] (
    [SkillLevelId] INT              IDENTITY (1, 1) NOT NULL,
    [SkillLevel]   INT              NOT NULL,
    [Title]        NVARCHAR (100)   NOT NULL,
    [Description]  NVARCHAR (MAX)   NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeSkillLevel_1] PRIMARY KEY CLUSTERED ([SkillLevelId] ASC)
);

