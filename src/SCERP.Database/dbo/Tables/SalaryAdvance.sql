CREATE TABLE [dbo].[SalaryAdvance] (
    [SalaryAdvanceId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]      UNIQUEIDENTIFIER NOT NULL,
    [Amount]          DECIMAL (18, 2)  NOT NULL,
    [ReceivedDate]    DATETIME         NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              CONSTRAINT [DF_SalaryAdvance_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_AdvanceAmount] PRIMARY KEY CLUSTERED ([SalaryAdvanceId] ASC),
    CONSTRAINT [FK_SalaryAdvance_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

