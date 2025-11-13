CREATE TABLE [dbo].[Acc_GLAccounts_Hidden] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [ControlCode]    NUMERIC (18)    NULL,
    [AccountCode]    NUMERIC (18)    NULL,
    [AccountName]    NVARCHAR (500)  NULL,
    [BalanceType]    NVARCHAR (50)   NULL,
    [OpeningBalance] NUMERIC (18, 2) NULL,
    [AccountType]    NVARCHAR (50)   NULL,
    [IsActive]       BIT             NULL,
    CONSTRAINT [PK_Acc_GLAccounts_Hidden] PRIMARY KEY CLUSTERED ([Id] ASC)
);

