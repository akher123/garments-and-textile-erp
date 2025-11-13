CREATE TABLE [dbo].[CommCashBbLcDetails] (
    [CashBbLcDetailsId] INT            IDENTITY (1, 1) NOT NULL,
    [BbLcId]            INT            NOT NULL,
    [Item]              NVARCHAR (100) NULL,
    [Quantity]          FLOAT (53)     NULL,
    [Rate]              FLOAT (53)     NULL,
    [Remarks]           NVARCHAR (100) NULL,
    CONSTRAINT [PK_CommCashBbLcDetails] PRIMARY KEY CLUSTERED ([CashBbLcDetailsId] ASC),
    CONSTRAINT [FK_CommCashBbLcDetails_CommCashBbLcInfo] FOREIGN KEY ([BbLcId]) REFERENCES [dbo].[CommCashBbLcInfo] ([BbLcId])
);

