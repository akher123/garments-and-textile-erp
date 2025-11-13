CREATE TABLE [dbo].[Acc_ReportControlTrialBalance] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [GroupCode]      NUMERIC (18)    NULL,
    [GroupName]      NVARCHAR (100)  NULL,
    [SubGroupCode]   NUMERIC (18)    NULL,
    [SubGroupName]   NVARCHAR (100)  NULL,
    [SubControlCode] NUMERIC (18)    NULL,
    [SubControlName] NVARCHAR (100)  NULL,
    [ControlCode]    NUMERIC (18)    NULL,
    [ControlName]    NVARCHAR (100)  NULL,
    [GLId]           INT             NULL,
    [GLCode]         NUMERIC (18)    NULL,
    [GLName]         NVARCHAR (100)  NULL,
    [OpeningBalance] DECIMAL (18, 2) NULL,
    [Debit]          DECIMAL (18, 2) NULL,
    [Credit]         DECIMAL (18, 2) NULL,
    [ClosingBalance] DECIMAL (18, 2) NULL,
    [IsActive]       BIT             NULL,
    CONSTRAINT [PK_Acc_ReportControlTrialBalance] PRIMARY KEY CLUSTERED ([Id] ASC)
);

