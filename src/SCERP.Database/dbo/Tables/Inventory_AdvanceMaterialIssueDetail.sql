CREATE TABLE [dbo].[Inventory_AdvanceMaterialIssueDetail] (
    [AdvanceMaterialIssueDetailId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [AdvanceMaterialIssueId]       BIGINT          NOT NULL,
    [CompId]                       VARCHAR (3)     NOT NULL,
    [ItemId]                       INT             NOT NULL,
    [ColorRefId]                   VARCHAR (4)     NOT NULL,
    [SizeRefId]                    VARCHAR (4)     NOT NULL,
    [IssueQty]                     DECIMAL (18, 5) NOT NULL,
    [IssueRate]                    DECIMAL (18, 5) NOT NULL,
    [FColorRefId]                  VARCHAR (4)     NULL,
    [QtyInBag]                     NUMERIC (18, 5) NULL,
    [Wrapper]                      VARCHAR (50)    NULL,
    [PurchaseOrderDetailId]        BIGINT          NULL,
    [GSizeRefId]                   VARCHAR (4)     NULL,
    [GColorRefId]                  VARCHAR (4)     NULL,
    CONSTRAINT [PK_Inventory_AdvanceMaterialIssueDetail] PRIMARY KEY CLUSTERED ([AdvanceMaterialIssueDetailId] ASC),
    CONSTRAINT [FK_Inventory_AdvanceMaterialIssueDetail_Inventory_AdvanceMaterialIssue] FOREIGN KEY ([AdvanceMaterialIssueId]) REFERENCES [dbo].[Inventory_AdvanceMaterialIssue] ([AdvanceMaterialIssueId])
);

