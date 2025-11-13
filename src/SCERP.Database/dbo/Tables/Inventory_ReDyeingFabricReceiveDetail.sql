CREATE TABLE [dbo].[Inventory_ReDyeingFabricReceiveDetail] (
    [ReDyeingFabricReceiveDetailId] BIGINT     IDENTITY (1, 1) NOT NULL,
    [ReDyeingFabricReceiveId]       BIGINT     NOT NULL,
    [BatchId]                       BIGINT     NOT NULL,
    [BatchDetailId]                 BIGINT     NOT NULL,
    [RQty]                          FLOAT (53) NOT NULL,
    CONSTRAINT [PK_Inventory_ReDyeingFabricReceiveDetail] PRIMARY KEY CLUSTERED ([ReDyeingFabricReceiveDetailId] ASC),
    CONSTRAINT [FK_Inventory_ReDyeingFabricReceiveDetail_Inventory_ReDyeingFabricReceive] FOREIGN KEY ([ReDyeingFabricReceiveId]) REFERENCES [dbo].[Inventory_ReDyeingFabricReceive] ([ReDyeingFabricReceiveId])
);

