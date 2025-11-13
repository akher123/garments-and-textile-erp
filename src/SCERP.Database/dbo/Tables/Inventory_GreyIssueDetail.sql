CREATE TABLE [dbo].[Inventory_GreyIssueDetail] (
    [GreyIssueDetailId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [GreyIssueId]       BIGINT       NOT NULL,
    [ProgramRegId]      VARCHAR (50) NOT NULL,
    [ItemCode]          VARCHAR (50) NOT NULL,
    [SizeRefId]         VARCHAR (4)  NULL,
    [ColorRefId]        VARCHAR (4)  NULL,
    [FinishSizeRefId]   VARCHAR (4)  NULL,
    [StLength]          VARCHAR (50) NULL,
    [GSM]               VARCHAR (15) NULL,
    [Qty]               FLOAT (53)   NOT NULL,
    [RollQty]           INT          NOT NULL,
    CONSTRAINT [PK_Inventory_GreyIssueDetail] PRIMARY KEY CLUSTERED ([GreyIssueDetailId] ASC),
    CONSTRAINT [FK_Inventory_GreyIssueDetail_Inventory_GreyIssue] FOREIGN KEY ([GreyIssueId]) REFERENCES [dbo].[Inventory_GreyIssue] ([GreyIssueId])
);

