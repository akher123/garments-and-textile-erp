CREATE TABLE [dbo].[EmployeeWorkShift] (
    [EmployeeWorkShiftId]   INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]            UNIQUEIDENTIFIER NOT NULL,
    [BranchUnitWorkShiftId] INT              NOT NULL,
    [ShiftDate]             DATETIME         NOT NULL,
    [StartDate]             DATETIME         NULL,
    [EndDate]               DATETIME         NULL,
    [Remarks]               NVARCHAR (MAX)   NULL,
    [Status]                BIT              NOT NULL,
    [CreatedDate]           DATETIME         NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    [EditedDate]            DATETIME         NULL,
    [EditedBy]              UNIQUEIDENTIFIER NULL,
    [IsActive]              BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeWorkShift] PRIMARY KEY CLUSTERED ([EmployeeWorkShiftId] ASC)
);

