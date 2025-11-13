CREATE TABLE [dbo].[DailyPresentReport] (
    [DailyPresentReportId] INT              IDENTITY (1, 1) NOT NULL,
    [DepartmentName]       NVARCHAR (100)   NULL,
    [DepartmentId]         INT              NULL,
    [SectionName]          NVARCHAR (100)   NULL,
    [SectionId]            INT              NULL,
    [LineName]             NVARCHAR (100)   NULL,
    [LineId]               INT              NULL,
    [BudgetManPower]       INT              NULL,
    [TotalEmployee]        INT              NULL,
    [Present]              INT              NULL,
    [Late]                 INT              NULL,
    [Absent]               INT              NULL,
    [Leave]                INT              NULL,
    [Balance]              INT              NULL,
    [Percent]              FLOAT (53)       NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_DailyPresentReport] PRIMARY KEY CLUSTERED ([DailyPresentReportId] ASC)
);

