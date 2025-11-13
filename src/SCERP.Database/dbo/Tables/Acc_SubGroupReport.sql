CREATE TABLE [dbo].[Acc_SubGroupReport] (
    [SubGroupReportId] INT             IDENTITY (1, 1) NOT NULL,
    [CompanyName]      NVARCHAR (100)  NULL,
    [CompanyAddress]   NVARCHAR (200)  NULL,
    [SubGroupId]       INT             NULL,
    [SubGroupCode]     NVARCHAR (50)   NULL,
    [SubGroupName]     NVARCHAR (100)  NULL,
    [ControlId]        INT             NULL,
    [ControlCode]      NVARCHAR (50)   NULL,
    [ControlName]      NVARCHAR (100)  NULL,
    [FromDate]         DATETIME        NULL,
    [ToDate]           DATETIME        NULL,
    [OpeningBalance]   DECIMAL (18, 2) NULL,
    [TotalDebit]       DECIMAL (18, 2) NULL,
    [TotalCredit]      DECIMAL (18, 2) NULL,
    [ClosingBalance]   DECIMAL (18, 2) NULL
);

