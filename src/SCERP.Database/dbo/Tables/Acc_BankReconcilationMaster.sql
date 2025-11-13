CREATE TABLE [dbo].[Acc_BankReconcilationMaster] (
    [Id]                INT              IDENTITY (1, 1) NOT NULL,
    [SectorId]          INT              NULL,
    [FinancialPeriodId] INT              NULL,
    [GLID]              INT              NULL,
    [FromDate]          DATE             NULL,
    [ToDate]            DATE             NULL,
    [CDT]               DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EDT]               DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NULL,
    CONSTRAINT [PK_Acc_BankReconcilationMaster] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Acc_BankReconcilationMaster_Acc_CompanySector] FOREIGN KEY ([SectorId]) REFERENCES [dbo].[Acc_CompanySector] ([Id]),
    CONSTRAINT [FK_Acc_BankReconcilationMaster_Acc_FinancialPeriod] FOREIGN KEY ([FinancialPeriodId]) REFERENCES [dbo].[Acc_FinancialPeriod] ([Id]),
    CONSTRAINT [FK_Acc_BankReconcilationMaster_Acc_GLAccounts] FOREIGN KEY ([GLID]) REFERENCES [dbo].[Acc_GLAccounts_Back] ([Id])
);

