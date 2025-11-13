CREATE TABLE [dbo].[Inventory_RedyeingFabricIssueDetail] (
    [RedyeingFabricIssueDetailId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [RedyeingFabricIssueId]       BIGINT        NOT NULL,
    [BatchId]                     BIGINT        NOT NULL,
    [BatchDetailId]               BIGINT        NOT NULL,
    [ReprocessQty]                FLOAT (53)    NOT NULL,
    [FinishQty]                   FLOAT (53)    NOT NULL,
    [NoRole]                      INT           NOT NULL,
    [Remarks]                     VARCHAR (150) NULL,
    CONSTRAINT [PK_Inventory_RedyeingFabricIssueDetail] PRIMARY KEY CLUSTERED ([RedyeingFabricIssueDetailId] ASC),
    CONSTRAINT [FK_Inventory_RedyeingFabricIssueDetail_Inventory_RedyeingFabricIssueDetail] FOREIGN KEY ([RedyeingFabricIssueId]) REFERENCES [dbo].[Inventory_RedyeingFabricIssue] ([RedyeingFabricIssueId])
);

