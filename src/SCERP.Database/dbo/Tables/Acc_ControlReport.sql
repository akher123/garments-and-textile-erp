CREATE TABLE [dbo].[Acc_ControlReport] (
    [ControlReportId] INT             IDENTITY (1, 1) NOT NULL,
    [CompanyName]     NVARCHAR (100)  NULL,
    [CompanyAddress]  NVARCHAR (200)  NULL,
    [ControlId]       INT             NULL,
    [ControlCode]     NVARCHAR (50)   NULL,
    [ControlName]     NVARCHAR (100)  NULL,
    [FromDate]        DATETIME        NULL,
    [ToDate]          DATETIME        NULL,
    [GLAccountId]     INT             NULL,
    [GLAccountCode]   NVARCHAR (50)   NULL,
    [GLAccountName]   NVARCHAR (100)  NULL,
    [OpeningBalance]  DECIMAL (18, 2) NULL,
    [TotalDebit]      DECIMAL (18, 2) NULL,
    [TotalCredit]     DECIMAL (18, 2) NULL,
    [ClosingBalance]  DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_Acc_ControlReport] PRIMARY KEY CLUSTERED ([ControlReportId] ASC)
);

