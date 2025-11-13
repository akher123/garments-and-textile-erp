CREATE TABLE [dbo].[PROD_KnittingRollIssueDetail] (
    [KnittingRollIssueDetailId] BIGINT     IDENTITY (1, 1) NOT NULL,
    [KnittingRollIssueId]       INT        NOT NULL,
    [KnittingRollId]            BIGINT     NOT NULL,
    [RollQty]                   FLOAT (53) NOT NULL,
    CONSTRAINT [PK_PROD_KnittingRollIssueDetail] PRIMARY KEY CLUSTERED ([KnittingRollIssueDetailId] ASC),
    CONSTRAINT [FK_PROD_KnittingRollIssueDetail_PROD_KnittingRoll] FOREIGN KEY ([KnittingRollId]) REFERENCES [dbo].[PROD_KnittingRoll] ([KnittingRollId]),
    CONSTRAINT [FK_PROD_KnittingRollIssueDetail_PROD_KnittingRollIssueDetail] FOREIGN KEY ([KnittingRollIssueId]) REFERENCES [dbo].[PROD_KnittingRollIssue] ([KnittingRollIssueId])
);

