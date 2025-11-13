CREATE TABLE [dbo].[CMSalaryInfo] (
    [CMSalaryId]          INT             IDENTITY (1, 1) NOT NULL,
    [Year]                INT             NULL,
    [Month]               INT             NULL,
    [TotalDays]           INT             NULL,
    [TotalSalary]         DECIMAL (18, 2) NULL,
    [KnitingWorkerSalary] DECIMAL (18, 2) NULL,
    [AdminSalary]         DECIMAL (18, 2) NULL,
    [Percent]             DECIMAL (18, 5) NULL,
    CONSTRAINT [PK_CMSalaryInfo] PRIMARY KEY CLUSTERED ([CMSalaryId] ASC)
);

