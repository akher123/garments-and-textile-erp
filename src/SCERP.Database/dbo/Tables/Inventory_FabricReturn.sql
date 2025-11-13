CREATE TABLE [dbo].[Inventory_FabricReturn] (
    [FabricReturnId]  BIGINT           IDENTITY (1, 1) NOT NULL,
    [ProgramId]       BIGINT           NOT NULL,
    [ReturnChallanNo] VARCHAR (15)     NOT NULL,
    [ReturnDate]      DATETIME         NOT NULL,
    [FabQty]          FLOAT (53)       NOT NULL,
    [ReturnYarnQty]   FLOAT (53)       NOT NULL,
    [WstYarnQty]      FLOAT (53)       NOT NULL,
    [Remarks]         VARCHAR (150)    NULL,
    [ReceivedBy]      UNIQUEIDENTIFIER NULL,
    [QtyInPcs]        INT              NULL,
    [CompId]          VARCHAR (3)      NOT NULL,
    [ProgramDetailId] BIGINT           NULL,
    [ProcessRefId]    VARCHAR (3)      NULL,
    CONSTRAINT [PK_Inventory_FabricReturn] PRIMARY KEY CLUSTERED ([FabricReturnId] ASC),
    CONSTRAINT [FK_Inventory_FabricReturn_Inventory_FabricReturn] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[PLAN_Program] ([ProgramId])
);

