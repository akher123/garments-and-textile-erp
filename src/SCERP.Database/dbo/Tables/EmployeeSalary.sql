CREATE TABLE [dbo].[EmployeeSalary] (
    [Id]                     INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]             UNIQUEIDENTIFIER NOT NULL,
    [GrossSalary]            DECIMAL (18, 2)  NOT NULL,
    [BasicSalary]            DECIMAL (18, 2)  NOT NULL,
    [HouseRent]              DECIMAL (18, 2)  NOT NULL,
    [MedicalAllowance]       DECIMAL (18, 2)  NOT NULL,
    [FoodAllowance]          DECIMAL (18, 2)  NULL,
    [Conveyance]             DECIMAL (18, 2)  NOT NULL,
    [EntertainmentAllowance] DECIMAL (18, 2)  NULL,
    [FromDate]               DATETIME         NOT NULL,
    [ToDate]                 DATETIME         NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeSalary] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeSalary_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE CASCADE
);

