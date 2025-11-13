CREATE TABLE [dbo].[EmployeeGrade] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (100)   NOT NULL,
    [NameInBengali]  NVARCHAR (100)   NOT NULL,
    [EmployeeTypeId] INT              NOT NULL,
    [Description]    NVARCHAR (MAX)   NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeGrade] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeGrade_EmployeeType] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [dbo].[EmployeeType] ([Id])
);

