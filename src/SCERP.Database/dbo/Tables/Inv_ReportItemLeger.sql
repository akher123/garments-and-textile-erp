CREATE TABLE [dbo].[Inv_ReportItemLeger] (
    [Ref]             VARCHAR (20)   NOT NULL,
    [TransactionDate] DATE           NOT NULL,
    [TransactionType] NVARCHAR (100) NOT NULL,
    [Quantity]        FLOAT (53)     NOT NULL,
    [UnitPrice]       FLOAT (53)     NOT NULL,
    [Amount]          FLOAT (53)     NULL,
    [QuantityL]       FLOAT (53)     NOT NULL,
    [UnitPriceL]      FLOAT (53)     NOT NULL,
    [AmountL]         FLOAT (53)     NULL,
    [QuantityB]       FLOAT (53)     NOT NULL,
    [UnitPriceB]      FLOAT (53)     NOT NULL,
    [AmountB]         FLOAT (53)     NULL,
    [TransactionName] VARCHAR (16)   NOT NULL,
    [Invoice]         VARCHAR (500)  NULL
);

