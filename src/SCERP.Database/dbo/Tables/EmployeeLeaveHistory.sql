CREATE TABLE [dbo].[EmployeeLeaveHistory] (
    [EmployeeLeaveHistoryId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]             UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId]         NVARCHAR (100)   NULL,
    [Year]                   INT              NOT NULL,
    [LeaveTypeId]            INT              NOT NULL,
    [NoOfAllowableLeaveDays] INT              NULL,
    [NoOfConsumedLeaveDays]  INT              NULL,
    [NoOfRemainingLeaveDays] INT              NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeLeaveHistory] PRIMARY KEY CLUSTERED ([EmployeeLeaveHistoryId] ASC),
    CONSTRAINT [FK_EmployeeLeaveHistory_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]),
    CONSTRAINT [FK_EmployeeLeaveHistory_LeaveType] FOREIGN KEY ([LeaveTypeId]) REFERENCES [dbo].[LeaveType] ([Id])
);

