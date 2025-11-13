CREATE TABLE [dbo].[Inventory_GreyIssue] (
    [GreyIssueId]     BIGINT           IDENTITY (1, 1) NOT NULL,
    [RefId]           CHAR (8)         NOT NULL,
    [PartyId]         BIGINT           NOT NULL,
    [ChallanNo]       VARCHAR (50)     NULL,
    [ChallanDate]     DATETIME         NULL,
    [Through]         VARCHAR (250)    NULL,
    [VheicalNo]       VARCHAR (150)    NULL,
    [Mobile]          VARCHAR (150)    NULL,
    [Remarks]         VARCHAR (250)    NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [CompId]          CHAR (3)         NOT NULL,
    [IsApproved]      BIT              NOT NULL,
    [ApprovedBy]      UNIQUEIDENTIFIER NULL,
    [AuditeBy]        UNIQUEIDENTIFIER NULL,
    [IsAudited]       BIT              NULL,
    [VoucherMasterId] BIGINT           NULL,
    [Posted]          CHAR (1)         NULL,
    CONSTRAINT [PK_Inventory_GreyIssue] PRIMARY KEY CLUSTERED ([GreyIssueId] ASC),
    CONSTRAINT [FK_Inventory_GreyIssue_Inventory_GreyIssue] FOREIGN KEY ([PartyId]) REFERENCES [dbo].[Party] ([PartyId])
);

