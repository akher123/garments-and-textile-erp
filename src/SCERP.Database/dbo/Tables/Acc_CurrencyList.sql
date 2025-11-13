CREATE TABLE [dbo].[Acc_CurrencyList] (
    [CurrencyListId] INT           IDENTITY (1, 1) NOT NULL,
    [Cname]          NVARCHAR (50) NULL,
    [Csymbol]        NVARCHAR (10) NULL,
    CONSTRAINT [PK_Acc_CurrencyList] PRIMARY KEY CLUSTERED ([CurrencyListId] ASC)
);

