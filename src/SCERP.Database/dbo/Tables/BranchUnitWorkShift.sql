CREATE TABLE [dbo].[BranchUnitWorkShift] (
    [BranchUnitWorkShiftId] INT              IDENTITY (1, 1) NOT NULL,
    [BranchUnitId]          INT              NOT NULL,
    [WorkShiftId]           INT              NOT NULL,
    [Status]                INT              NOT NULL,
    [FromDate]              DATETIME         NOT NULL,
    [ToDate]                DATETIME         NULL,
    [Description]           NVARCHAR (MAX)   NULL,
    [CreatedDate]           DATETIME         NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    [EditedDate]            DATETIME         NULL,
    [EditedBy]              UNIQUEIDENTIFIER NULL,
    [IsActive]              BIT              NOT NULL,
    CONSTRAINT [PK_BranchUnitWorkShift] PRIMARY KEY CLUSTERED ([BranchUnitWorkShiftId] ASC),
    CONSTRAINT [FK_BranchUnitWorkShift_BranchUnit] FOREIGN KEY ([BranchUnitId]) REFERENCES [dbo].[BranchUnit] ([BranchUnitId]),
    CONSTRAINT [FK_BranchUnitWorkShift_WorkShift] FOREIGN KEY ([WorkShiftId]) REFERENCES [dbo].[WorkShift] ([WorkShiftId])
);

