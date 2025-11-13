CREATE TABLE [dbo].[EmployeeLeaveDetail] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeLeaveId] INT              NOT NULL,
    [EmployeeId]      UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId]  NVARCHAR (100)   NOT NULL,
    [ConsumedDate]    DATETIME         NOT NULL,
    [SubmitDate]      DATETIME         NOT NULL,
    [LeaveTypeId]     INT              NOT NULL,
    [LeaveTypeTitle]  NVARCHAR (100)   NOT NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeLeaveDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeLeaveDetail_EmployeeLeave] FOREIGN KEY ([EmployeeLeaveId]) REFERENCES [dbo].[EmployeeLeave] ([Id])
);

