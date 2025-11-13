CREATE TABLE [dbo].[Inventory_RejectYarnIssue] (
    [RejectYarnIssueId] INT           IDENTITY (1, 1) NOT NULL,
    [PartyId]           BIGINT        NOT NULL,
    [RefId]             VARCHAR (5)   NOT NULL,
    [IssueDate]         DATETIME      NOT NULL,
    [ChallanNo]         VARCHAR (50)  NULL,
    [IssueType]         CHAR (1)      NOT NULL,
    [Remarks]           VARCHAR (250) NULL,
    CONSTRAINT [PK_Inventory_RejectYarnIssue] PRIMARY KEY CLUSTERED ([RejectYarnIssueId] ASC),
    CONSTRAINT [FK_Inventory_RejectYarnIssue_Party] FOREIGN KEY ([PartyId]) REFERENCES [dbo].[Party] ([PartyId])
);

