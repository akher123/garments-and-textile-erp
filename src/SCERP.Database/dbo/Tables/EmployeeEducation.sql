CREATE TABLE [dbo].[EmployeeEducation] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]         UNIQUEIDENTIFIER NOT NULL,
    [EducationLevelId]   INT              NOT NULL,
    [ExamTitle]          NVARCHAR (100)   NOT NULL,
    [ExamTitleInBengali] NVARCHAR (100)   NULL,
    [Institute]          NVARCHAR (100)   NULL,
    [InstituteInBengali] NVARCHAR (100)   NULL,
    [Result]             NVARCHAR (100)   NULL,
    [ResultInBengali]    NVARCHAR (100)   NULL,
    [PassingYear]        INT              NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              CONSTRAINT [DF_EmployeeEducation_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_EmployeeEducation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeEducation_EducationLevel] FOREIGN KEY ([EducationLevelId]) REFERENCES [dbo].[EducationLevel] ([Id]),
    CONSTRAINT [FK_EmployeeEducation_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE CASCADE
);

