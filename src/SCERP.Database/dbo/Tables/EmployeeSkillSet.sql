CREATE TABLE [dbo].[EmployeeSkillSet] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]   UNIQUEIDENTIFIER NOT NULL,
    [SkillSetId]   INT              NOT NULL,
    [SkillLevelId] INT              NOT NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeSkillSet] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeSkillSet_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]),
    CONSTRAINT [FK_EmployeeSkillSet_SkillLevel] FOREIGN KEY ([SkillLevelId]) REFERENCES [dbo].[SkillLevel] ([SkillLevelId]),
    CONSTRAINT [FK_EmployeeSkillSet_SkillSet1] FOREIGN KEY ([SkillSetId]) REFERENCES [dbo].[SkillSet] ([Id])
);

