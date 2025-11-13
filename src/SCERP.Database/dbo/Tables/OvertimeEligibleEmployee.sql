CREATE TABLE [dbo].[OvertimeEligibleEmployee] (
    [OvertimeEligibleEmployeeId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]                 UNIQUEIDENTIFIER NOT NULL,
    [OvertimeDate]               DATETIME         NOT NULL,
    [OvertimeHour]               DECIMAL (18, 2)  NOT NULL,
    [Remarks]                    NVARCHAR (MAX)   NULL,
    [Status]                     BIT              NOT NULL,
    [CreatedDate]                DATETIME         NULL,
    [CreatedBy]                  UNIQUEIDENTIFIER NULL,
    [EditedDate]                 DATETIME         NULL,
    [EditedBy]                   UNIQUEIDENTIFIER NULL,
    [IsActive]                   BIT              NOT NULL,
    CONSTRAINT [PK_OvertimeEligibleEmployee] PRIMARY KEY CLUSTERED ([OvertimeEligibleEmployeeId] ASC),
    CONSTRAINT [FK_OvertimeEligibleEmployee_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

