CREATE TABLE [dbo].[Acc_OpeningClosing] (
    [Id]        BIGINT           IDENTITY (1, 1) NOT NULL,
    [SectorId]  INT              NULL,
    [FpId]      INT              NULL,
    [GlId]      INT              NULL,
    [Debit]     NUMERIC (18, 2)  NULL,
    [Credit]    NUMERIC (18, 2)  NULL,
    [CDT]       DATETIME         NULL,
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [EDT]       DATETIME         NULL,
    [EditedBy]  UNIQUEIDENTIFIER NULL,
    [IsActive]  BIT              NULL,
    CONSTRAINT [PK_Acc_OpeningClosing] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Acc_OpeningClosing_Acc_CompanySector] FOREIGN KEY ([SectorId]) REFERENCES [dbo].[Acc_CompanySector] ([Id]),
    CONSTRAINT [FK_Acc_OpeningClosing_Acc_FinancialPeriod] FOREIGN KEY ([FpId]) REFERENCES [dbo].[Acc_FinancialPeriod] ([Id]),
    CONSTRAINT [FK_Acc_OpeningClosing_Acc_GLAccounts] FOREIGN KEY ([GlId]) REFERENCES [dbo].[Acc_GLAccounts_Back] ([Id])
);

