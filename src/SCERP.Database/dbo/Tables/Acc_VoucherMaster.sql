CREATE TABLE [dbo].[Acc_VoucherMaster] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [VoucherType]       NVARCHAR (50)  NOT NULL,
    [VoucherNo]         BIGINT         NOT NULL,
    [VoucherRefNo]      NVARCHAR (50)  NULL,
    [VoucherDate]       DATE           NOT NULL,
    [CheckNo]           NVARCHAR (50)  NULL,
    [CheckDate]         NVARCHAR (50)  NULL,
    [Particulars]       NVARCHAR (MAX) NULL,
    [TotalAmountInWord] NVARCHAR (200) NULL,
    [SectorId]          INT            NULL,
    [CostCentreId]      INT            NULL,
    [FinancialPeriodId] INT            NULL,
    [ActiveCurrencyId]  INT            NULL,
    [IsActive]          BIT            NULL,
    [IntRefId]          VARCHAR (10)   NULL,
    [IntType]           INT            NULL,
    CONSTRAINT [PK_Acc_VoucherMaster] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Acc_VoucherMaster_Acc_CompanySector] FOREIGN KEY ([SectorId]) REFERENCES [dbo].[Acc_CompanySector] ([Id]),
    CONSTRAINT [FK_Acc_VoucherMaster_Acc_FinancialPeriod] FOREIGN KEY ([FinancialPeriodId]) REFERENCES [dbo].[Acc_FinancialPeriod] ([Id])
);

