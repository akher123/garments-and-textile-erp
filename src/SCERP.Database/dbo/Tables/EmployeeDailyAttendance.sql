CREATE TABLE [dbo].[EmployeeDailyAttendance] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]          UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId]      NVARCHAR (100)   NOT NULL,
    [TransactionDateTime] DATETIME         NOT NULL,
    [FunctionKey]         TINYINT          NULL,
    [IsFromMachine]       BIT              NOT NULL,
    [Remarks]             NVARCHAR (MAX)   NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeDailyAttendance] PRIMARY KEY CLUSTERED ([Id] ASC)
);

