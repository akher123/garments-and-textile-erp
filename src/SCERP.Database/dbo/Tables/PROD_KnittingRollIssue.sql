CREATE TABLE [dbo].[PROD_KnittingRollIssue] (
    [KnittingRollIssueId] INT              IDENTITY (1, 1) NOT NULL,
    [IssueRefNo]          VARCHAR (8)      NOT NULL,
    [BuyerRefId]          VARCHAR (3)      NOT NULL,
    [OrderNo]             VARCHAR (12)     NOT NULL,
    [OrderStyleRefId]     VARCHAR (7)      NOT NULL,
    [IssueDate]           DATETIME         NOT NULL,
    [BatchNo]             VARCHAR (50)     NOT NULL,
    [Qty]                 FLOAT (53)       NOT NULL,
    [CompId]              VARCHAR (3)      NOT NULL,
    [Remarks]             VARCHAR (150)    NULL,
    [ProgramRefId]        VARCHAR (10)     NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [Editedby]            UNIQUEIDENTIFIER NULL,
    [CreatedDate]         DATETIME         NULL,
    [EditedDate]          DATETIME         NULL,
    [IsRecived]           BIT              NULL,
    [ReceivedBy]          UNIQUEIDENTIFIER NULL,
    [ChallanType]         INT              NOT NULL,
    [VoucherMasterId]     BIGINT           NULL,
    [Posted]              CHAR (1)         NULL,
    CONSTRAINT [PK_KnittingRollIssue] PRIMARY KEY CLUSTERED ([KnittingRollIssueId] ASC)
);

