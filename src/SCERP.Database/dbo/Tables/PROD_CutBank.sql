CREATE TABLE [dbo].[PROD_CutBank] (
    [CutBankId]       BIGINT      IDENTITY (1, 1) NOT NULL,
    [OrderStyleRefId] VARCHAR (7) NOT NULL,
    [ColorRefId]      VARCHAR (4) NOT NULL,
    [SizeRefId]       VARCHAR (4) NOT NULL,
    [ComponentRefId]  VARCHAR (3) NOT NULL,
    [OrderQty]        INT         NULL,
    [SolidQty]        INT         NULL,
    [PrintRcvQty]     INT         NULL,
    [EmbRcvQty]       INT         NULL,
    [PrintRejQty]     INT         NULL,
    [EmbRejQty]       INT         NULL,
    [FabricRejQty]    INT         NULL,
    [CutFQty]         INT         NULL,
    [BankQty]         INT         NULL,
    [BalanceQty]      INT         NULL,
    [CompId]          VARCHAR (4) NULL,
    [ComponentType]   CHAR (1)    NULL,
    CONSTRAINT [PK_PROD_CutBank] PRIMARY KEY CLUSTERED ([CutBankId] ASC)
);

