CREATE TABLE [dbo].[MIS_SalarySummaryDaily] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [TransactionDate] DATE            NOT NULL,
    [DepartmentId]    INT             NULL,
    [DepartmentName]  NVARCHAR (100)  NULL,
    [NetAmount]       DECIMAL (18, 2) NULL,
    [ExtraOTAmount]   DECIMAL (18, 2) NULL,
    [WeekendOTAmount] DECIMAL (18, 2) NULL,
    [HolidayOTAmount] DECIMAL (18, 2) NULL,
    [Percentage]      DECIMAL (18, 2) NULL,
    [GroupCode]       CHAR (1)        NULL,
    CONSTRAINT [PK_MIS_SalarySummaryDaily] PRIMARY KEY CLUSTERED ([Id] ASC)
);

