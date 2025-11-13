CREATE TABLE [dbo].[Acc_Currency] (
    [CurrencyId]      INT             IDENTITY (1, 1) NOT NULL,
    [FirstCurName]    NVARCHAR (50)   NULL,
    [FirstCurValue]   NUMERIC (18, 2) NOT NULL,
    [FirstCurSymbol]  NVARCHAR (50)   NULL,
    [SecendCurName]   NVARCHAR (50)   NULL,
    [SecendCurValue]  NUMERIC (18, 2) NOT NULL,
    [SecendCurSymbol] NVARCHAR (50)   NULL,
    [ThirdCurName]    NVARCHAR (50)   NULL,
    [ThirdCurValue]   NUMERIC (18, 2) NOT NULL,
    [ThirdCurSymbol]  NVARCHAR (50)   NULL,
    [CurDate]         DATETIME        NULL,
    [ActiveStatus]    BIT             NOT NULL,
    CONSTRAINT [PK_Acc_Currency] PRIMARY KEY CLUSTERED ([CurrencyId] ASC)
);

