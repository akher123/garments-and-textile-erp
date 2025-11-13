CREATE TABLE [dbo].[WorkShiftRosterHistory] (
    [WorkShiftRosterHistoryId] INT              IDENTITY (1, 1) NOT NULL,
    [Date]                     DATETIME         NULL,
    [UnitName]                 NVARCHAR (50)    NULL,
    [GroupName]                NVARCHAR (50)    NULL,
    [ShiftName]                NVARCHAR (50)    NULL,
    [BranchUnitWorkShiftId]    INT              NULL,
    [CreatedDate]              DATETIME         NULL,
    [CreatedBy]                UNIQUEIDENTIFIER NULL,
    [EditedDate]               DATETIME         NULL,
    [EditedBy]                 UNIQUEIDENTIFIER NULL,
    [IsActive]                 BIT              NOT NULL
);

