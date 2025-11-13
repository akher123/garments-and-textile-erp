CREATE TABLE [dbo].[HeadOfDepartment] (
    [HeadOfDepartmentId]     INT              IDENTITY (1, 1) NOT NULL,
    [BranchUnitDepartmentId] INT              NOT NULL,
    [EmployeeId]             UNIQUEIDENTIFIER NOT NULL,
    [FromDate]               DATETIME         NOT NULL,
    [ToDate]                 DATETIME         NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_HeadOfDepartment] PRIMARY KEY CLUSTERED ([HeadOfDepartmentId] ASC),
    CONSTRAINT [FK_HeadOfDepartment_BranchUnitDepartment] FOREIGN KEY ([BranchUnitDepartmentId]) REFERENCES [dbo].[BranchUnitDepartment] ([BranchUnitDepartmentId]),
    CONSTRAINT [FK_HeadOfDepartment_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

