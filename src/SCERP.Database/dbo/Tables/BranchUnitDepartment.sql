CREATE TABLE [dbo].[BranchUnitDepartment] (
    [BranchUnitDepartmentId] INT              IDENTITY (1, 1) NOT NULL,
    [BranchUnitId]           INT              NOT NULL,
    [UnitDepartmentId]       INT              NOT NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_BranchUnitDepartment] PRIMARY KEY CLUSTERED ([BranchUnitDepartmentId] ASC),
    CONSTRAINT [FK_BranchUnitDepartment_BranchUnit] FOREIGN KEY ([BranchUnitId]) REFERENCES [dbo].[BranchUnit] ([BranchUnitId]),
    CONSTRAINT [FK_BranchUnitDepartment_UnitDepartment] FOREIGN KEY ([UnitDepartmentId]) REFERENCES [dbo].[UnitDepartment] ([UnitDepartmentId])
);

