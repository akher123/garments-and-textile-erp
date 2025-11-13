CREATE TABLE [dbo].[LeaveSetting] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeTypeId] INT              NOT NULL,
    [LeaveTypeId]    INT              NOT NULL,
    [NoOfDays]       INT              NULL,
    [MaximumAtATime] INT              NULL,
    [BranchUnitId]   INT              NOT NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_LeaveSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeaveSetting_BranchUnit] FOREIGN KEY ([BranchUnitId]) REFERENCES [dbo].[BranchUnit] ([BranchUnitId]),
    CONSTRAINT [FK_LeaveSetting_EmployeeType] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [dbo].[EmployeeType] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_LeaveSetting_LeaveType] FOREIGN KEY ([LeaveTypeId]) REFERENCES [dbo].[LeaveType] ([Id])
);

