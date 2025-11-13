CREATE TABLE [dbo].[UnitDepartment] (
    [UnitDepartmentId] INT              IDENTITY (1, 1) NOT NULL,
    [UnitId]           INT              NOT NULL,
    [DepartmentId]     INT              NOT NULL,
    [CreatedDate]      DATETIME         NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]       DATETIME         NULL,
    [EditedBy]         UNIQUEIDENTIFIER NULL,
    [IsActive]         BIT              NOT NULL,
    CONSTRAINT [PK_UnitDepartment] PRIMARY KEY CLUSTERED ([UnitDepartmentId] ASC),
    CONSTRAINT [FK_UnitDepartment_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Department] ([Id]),
    CONSTRAINT [FK_UnitDepartment_Unit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[Unit] ([UnitId])
);

