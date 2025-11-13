CREATE TABLE [dbo].[Inventory_RejectYarnIssueDetail] (
    [RejectYarnIssueDetailId] INT        IDENTITY (1, 1) NOT NULL,
    [RejectYarnIssueId]       INT        NOT NULL,
    [MaterialReceiveDetailId] BIGINT     NOT NULL,
    [Qty]                     FLOAT (53) NOT NULL,
    CONSTRAINT [PK_Inventory_RejectYarnIssueDetail] PRIMARY KEY CLUSTERED ([RejectYarnIssueDetailId] ASC),
    CONSTRAINT [FK_Inventory_RejectYarnIssueDetail_Inventory_RejectYarnIssue] FOREIGN KEY ([RejectYarnIssueId]) REFERENCES [dbo].[Inventory_RejectYarnIssue] ([RejectYarnIssueId])
);

