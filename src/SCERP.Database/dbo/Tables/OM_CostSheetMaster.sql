CREATE TABLE [dbo].[OM_CostSheetMaster] (
    [CostSheetMasterId]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [CostSheetMasterRefId] VARCHAR (8)   NOT NULL,
    [BuyerId]              BIGINT        NOT NULL,
    [OrderNo]              VARCHAR (50)  NULL,
    [StyleNo]              VARCHAR (50)  NULL,
    [OrderQty]             INT           NULL,
    [Item]                 VARCHAR (100) NOT NULL,
    [Fabrication]          VARCHAR (100) NULL,
    [Color]                VARCHAR (50)  NULL,
    [Size]                 VARCHAR (20)  NULL,
    [YarnCount]            VARCHAR (50)  NULL,
    [Gsm]                  FLOAT (53)    NOT NULL,
    [ItemTypeId]           INT           NOT NULL,
    [CompId]               VARCHAR (3)   NOT NULL,
    [Remarks]              VARCHAR (150) NULL,
    CONSTRAINT [PK_OM_CostSheetMaster] PRIMARY KEY CLUSTERED ([CostSheetMasterId] ASC),
    CONSTRAINT [FK_OM_CostSheetMaster_OM_CostSheetMaster] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[OM_Buyer] ([BuyerId]),
    CONSTRAINT [FK_OM_CostSheetMaster_OM_ItemType] FOREIGN KEY ([ItemTypeId]) REFERENCES [dbo].[OM_ItemType] ([ItemTypeId])
);

