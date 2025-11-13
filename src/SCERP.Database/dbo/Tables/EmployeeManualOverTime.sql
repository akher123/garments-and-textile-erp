CREATE TABLE [dbo].[EmployeeManualOverTime] (
    [EmployeeManualOverTimeId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]               UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId]           NVARCHAR (20)    NOT NULL,
    [Date]                     DATETIME         NOT NULL,
    [OverTimeHours]            NUMERIC (18, 2)  NOT NULL,
    [CreatedDate]              DATETIME         NULL,
    [CreatedBy]                UNIQUEIDENTIFIER NULL,
    [EditedDate]               DATETIME         NULL,
    [EditedBy]                 UNIQUEIDENTIFIER NULL,
    [IsActive]                 BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeManualOverTime] PRIMARY KEY CLUSTERED ([EmployeeManualOverTimeId] ASC)
);

