CREATE TABLE [dbo].[Inventory_MaterialReceived] (
    [MaterialReceivedId]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [MaterialReceivedRefId] VARCHAR (8)   NOT NULL,
    [CompId]                VARCHAR (3)   NOT NULL,
    [GRN]                   VARCHAR (50)  NULL,
    [GEN]                   VARCHAR (50)  NULL,
    [ReceivedDate]          DATETIME      NOT NULL,
    [ChallanNo]             VARCHAR (50)  NOT NULL,
    [ChallanDate]           DATETIME      NULL,
    [SupplierName]          VARCHAR (100) NULL,
    [BuyerName]             VARCHAR (100) NULL,
    [OrderNo]               VARCHAR (50)  NULL,
    [StyleNo]               VARCHAR (50)  NULL,
    [Article]               VARCHAR (100) NULL,
    [LCNo]                  VARCHAR (50)  NULL,
    [BillStatus]            VARCHAR (100) NULL,
    [Remarks]               VARCHAR (200) NULL,
    [RegisterType]          VARCHAR (20)  NULL,
    CONSTRAINT [PK_Inventory_MaterialReceived_1] PRIMARY KEY CLUSTERED ([MaterialReceivedId] ASC)
);

