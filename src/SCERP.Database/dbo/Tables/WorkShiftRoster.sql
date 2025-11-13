CREATE TABLE [dbo].[WorkShiftRoster] (
    [WorkShiftRosterId]     INT              IDENTITY (1, 1) NOT NULL,
    [UnitName]              NVARCHAR (50)    NULL,
    [GroupName]             NVARCHAR (50)    NULL,
    [ShiftName]             NVARCHAR (50)    NULL,
    [FromDate]              DATETIME         NULL,
    [BranchUnitWorkShiftId] INT              NULL,
    [CreatedDate]           DATETIME         NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    [EditedDate]            DATETIME         NULL,
    [EditedBy]              UNIQUEIDENTIFIER NULL,
    [IsActive]              BIT              NOT NULL,
    CONSTRAINT [PK_WorkShiftRoster] PRIMARY KEY CLUSTERED ([WorkShiftRosterId] ASC)
);

