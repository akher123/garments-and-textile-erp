CREATE TABLE [dbo].[Inventory_ReDyeingFabricReceive] (
    [ReDyeingFabricReceiveId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [RefNo]                   VARCHAR (6)    NULL,
    [ChallanDate]             DATETIME       NULL,
    [CompId]                  VARCHAR (3)    NOT NULL,
    [PartyId]                 BIGINT         NOT NULL,
    [ReceiveDate]             DATETIME       NOT NULL,
    [GatEntryNo]              VARCHAR (50)   NULL,
    [ChallanNo]               VARCHAR (50)   NULL,
    [Remarks]                 NVARCHAR (250) NULL,
    CONSTRAINT [PK_Inventory_ReDyeingFabricReceive] PRIMARY KEY CLUSTERED ([ReDyeingFabricReceiveId] ASC),
    CONSTRAINT [FK_Inventory_ReDyeingFabricReceive_Party] FOREIGN KEY ([PartyId]) REFERENCES [dbo].[Party] ([PartyId])
);

