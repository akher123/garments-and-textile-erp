CREATE TABLE [dbo].[DepartmentLine] (
    [DepartmentLineId]       INT              IDENTITY (1, 1) NOT NULL,
    [BranchUnitDepartmentId] INT              NOT NULL,
    [LineId]                 INT              NOT NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_DepartmentLine] PRIMARY KEY CLUSTERED ([DepartmentLineId] ASC),
    CONSTRAINT [FK_DepartmentLine_BranchUnitDepartment] FOREIGN KEY ([BranchUnitDepartmentId]) REFERENCES [dbo].[BranchUnitDepartment] ([BranchUnitDepartmentId]),
    CONSTRAINT [FK_DepartmentLine_Line] FOREIGN KEY ([LineId]) REFERENCES [dbo].[Line] ([LineId])
);

