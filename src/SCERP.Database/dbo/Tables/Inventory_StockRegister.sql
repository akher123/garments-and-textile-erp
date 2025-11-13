CREATE TABLE [dbo].[Inventory_StockRegister] (
    [AdvanceStoreLadgerId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompId]               VARCHAR (3)     NOT NULL,
    [ItemId]               INT             NOT NULL,
    [ColorRefId]           VARCHAR (4)     NOT NULL,
    [SizeRefId]            VARCHAR (4)     NOT NULL,
    [Quantity]             DECIMAL (18, 5) NOT NULL,
    [Rate]                 DECIMAL (18, 5) NOT NULL,
    [TransactionType]      INT             NOT NULL,
    [TransactionDate]      DATETIME        NOT NULL,
    [StoreId]              INT             NOT NULL,
    [SourceId]             BIGINT          NOT NULL,
    [ActionType]           INT             NOT NULL,
    CONSTRAINT [PK_Inventory_StockRegister] PRIMARY KEY CLUSTERED ([AdvanceStoreLadgerId] ASC)
);

