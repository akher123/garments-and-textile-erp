CREATE TABLE [dbo].[PROD_BatchRoll] (
    [BatchRollId]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [BatchId]             BIGINT           NOT NULL,
    [KnittingRollId]      BIGINT           NOT NULL,
    [Remarks]             VARCHAR (150)    NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NOT NULL,
    [EditdBy]             UNIQUEIDENTIFIER NULL,
    [CreatedDate]         DATETIME         NOT NULL,
    [EditedDate]          DATETIME         NULL,
    [CompId]              VARCHAR (3)      NULL,
    [KnittingRollIssueId] INT              NOT NULL,
    CONSTRAINT [PK_PROD_BatchRoll] PRIMARY KEY CLUSTERED ([BatchRollId] ASC),
    CONSTRAINT [FK_PROD_BatchRoll_Pro_Batch] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[Pro_Batch] ([BatchId]),
    CONSTRAINT [FK_PROD_BatchRoll_PROD_KnittingRoll] FOREIGN KEY ([KnittingRollId]) REFERENCES [dbo].[PROD_KnittingRoll] ([KnittingRollId]),
    CONSTRAINT [FK_PROD_BatchRoll_PROD_KnittingRollIssue] FOREIGN KEY ([KnittingRollIssueId]) REFERENCES [dbo].[PROD_KnittingRollIssue] ([KnittingRollIssueId])
);

