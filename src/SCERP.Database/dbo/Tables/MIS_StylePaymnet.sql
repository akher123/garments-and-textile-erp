CREATE TABLE [dbo].[MIS_StylePaymnet] (
    [StylePaymnetId]    INT           IDENTITY (1, 1) NOT NULL,
    [StylePaymentRefId] VARCHAR (8)   NOT NULL,
    [PayDate]           DATETIME      NULL,
    [BuyerRefId]        VARCHAR (4)   NOT NULL,
    [OrderNo]           VARCHAR (12)  NOT NULL,
    [OrderStyleRefId]   VARCHAR (7)   NOT NULL,
    [CostGroup]         VARCHAR (3)   NOT NULL,
    [PayAount]          FLOAT (53)    NOT NULL,
    [CompId]            VARCHAR (3)   NOT NULL,
    [Remarks]           VARCHAR (MAX) NULL,
    CONSTRAINT [PK_MIS_StylePaymnet] PRIMARY KEY CLUSTERED ([StylePaymnetId] ASC)
);

