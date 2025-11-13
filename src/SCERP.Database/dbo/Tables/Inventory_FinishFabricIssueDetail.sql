CREATE TABLE [dbo].[Inventory_FinishFabricIssueDetail] (
    [FinishFabricIssueDetailId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [FinishFabricIssueId]       BIGINT        NOT NULL,
    [BatchId]                   BIGINT        NOT NULL,
    [BatchDetailId]             BIGINT        NOT NULL,
    [FabQty]                    FLOAT (53)    NOT NULL,
    [NoOfRoll]                  INT           NOT NULL,
    [Remarks]                   VARCHAR (150) NOT NULL,
    [CompId]                    VARCHAR (3)   NULL,
    [GreyWt]                    FLOAT (53)    NOT NULL,
    [CcuffQty]                  FLOAT (53)    NULL,
    CONSTRAINT [PK_Inventory_FinishFabricIssueDetail] PRIMARY KEY CLUSTERED ([FinishFabricIssueDetailId] ASC),
    CONSTRAINT [FK_Inventory_FinishFabricIssueDetail_Inventory_FinishFabricIssue] FOREIGN KEY ([FinishFabricIssueId]) REFERENCES [dbo].[Inventory_FinishFabricIssue] ([FinishFabIssueId])
);

