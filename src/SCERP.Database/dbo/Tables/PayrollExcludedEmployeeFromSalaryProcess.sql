CREATE TABLE [dbo].[PayrollExcludedEmployeeFromSalaryProcess] (
    [ExcludedEmployeeFromSalaryProcessId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]                          UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId]                      NVARCHAR (100)   NOT NULL,
    [Year]                                INT              NOT NULL,
    [Month]                               INT              NOT NULL,
    [FromDate]                            DATETIME         NOT NULL,
    [ToDate]                              DATETIME         NOT NULL,
    [Remarks]                             NVARCHAR (MAX)   NULL,
    [CreatedDate]                         DATETIME         NULL,
    [CreatedBy]                           UNIQUEIDENTIFIER NULL,
    [EditedDate]                          DATETIME         NULL,
    [EditedBy]                            UNIQUEIDENTIFIER NULL,
    [IsActive]                            BIT              NOT NULL,
    CONSTRAINT [PK_PayrollExcludedEmployeeFromSalaryProcess] PRIMARY KEY CLUSTERED ([ExcludedEmployeeFromSalaryProcessId] ASC)
);

