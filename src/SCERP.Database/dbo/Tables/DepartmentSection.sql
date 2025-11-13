CREATE TABLE [dbo].[DepartmentSection] (
    [DepartmentSectionId]    INT              IDENTITY (1, 1) NOT NULL,
    [BranchUnitDepartmentId] INT              NOT NULL,
    [SectionId]              INT              NOT NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_DepartmentSection] PRIMARY KEY CLUSTERED ([DepartmentSectionId] ASC),
    CONSTRAINT [FK_DepartmentSection_BranchUnitDepartment] FOREIGN KEY ([BranchUnitDepartmentId]) REFERENCES [dbo].[BranchUnitDepartment] ([BranchUnitDepartmentId]),
    CONSTRAINT [FK_DepartmentSection_Section] FOREIGN KEY ([SectionId]) REFERENCES [dbo].[Section] ([SectionId])
);

