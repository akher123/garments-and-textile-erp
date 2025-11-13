CREATE TABLE [dbo].[Acc_GLAccounts_Back] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [ControlCode]    NUMERIC (18)    NOT NULL,
    [AccountCode]    NUMERIC (18)    NOT NULL,
    [AccountName]    NVARCHAR (500)  NOT NULL,
    [BalanceType]    NVARCHAR (50)   NULL,
    [OpeningBalance] NUMERIC (18, 2) NULL,
    [AccountType]    NVARCHAR (50)   NULL,
    [IsActive]       BIT             NULL,
    CONSTRAINT [PK_Acc_GLAccounts_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

