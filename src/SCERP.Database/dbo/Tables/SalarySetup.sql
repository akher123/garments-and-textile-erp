CREATE TABLE [dbo].[SalarySetup] (
    [Id]                     INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeGradeId]        INT              NOT NULL,
    [GrossSalary]            DECIMAL (18, 2)  NOT NULL,
    [BasicSalary]            DECIMAL (18, 2)  NOT NULL,
    [HouseRent]              DECIMAL (18, 2)  NOT NULL,
    [MedicalAllowance]       DECIMAL (18, 2)  NOT NULL,
    [Conveyance]             DECIMAL (18, 2)  NOT NULL,
    [FoodAllowance]          DECIMAL (18, 2)  NULL,
    [EntertainmentAllowance] DECIMAL (18, 2)  NULL,
    [Description]            NVARCHAR (MAX)   NULL,
    [FromDate]               DATETIME         NOT NULL,
    [ToDate]                 DATETIME         NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_SalarySetup_New] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalarySetup_EmployeeGrade] FOREIGN KEY ([EmployeeGradeId]) REFERENCES [dbo].[EmployeeGrade] ([Id])
);

