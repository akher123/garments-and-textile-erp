CREATE TABLE [dbo].[CommCashBbLcDetail] (
    [CashBbLcDetailsId] INT            IDENTITY (1, 1) NOT NULL,
    [BbLcId]            INT            NOT NULL,
    [Item]              NVARCHAR (100) NULL,
    [Quantity]          FLOAT (53)     NULL,
    [Rate]              FLOAT (53)     NULL,
    [Remarks]           NVARCHAR (100) NULL,
    CONSTRAINT [PK_CommCashBbLcDetail] PRIMARY KEY CLUSTERED ([CashBbLcDetailsId] ASC),
    CONSTRAINT [FK_CommCashBbLcDetail_CommCashBbLcInfo] FOREIGN KEY ([BbLcId]) REFERENCES [dbo].[CommCashBbLcInfo] ([BbLcId])
);

