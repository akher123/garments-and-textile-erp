CREATE TABLE [dbo].[EmployeeDesignation] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (100)   NOT NULL,
    [TitleInBengali] NVARCHAR (100)   NOT NULL,
    [EmployeeTypeId] INT              NOT NULL,
    [GradeId]        INT              NOT NULL,
    [Description]    NVARCHAR (MAX)   NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeDesignation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeDesignation_EmployeeGrade] FOREIGN KEY ([GradeId]) REFERENCES [dbo].[EmployeeGrade] ([Id]),
    CONSTRAINT [FK_EmployeeDesignation_EmployeeType] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [dbo].[EmployeeType] ([Id])
);

