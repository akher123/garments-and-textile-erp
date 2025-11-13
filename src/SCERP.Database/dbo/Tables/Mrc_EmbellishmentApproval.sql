CREATE TABLE [dbo].[Mrc_EmbellishmentApproval] (
    [EmbellishmentApprovalId] INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]             INT              NOT NULL,
    [ApprovalStatusId]        INT              NULL,
    [KeyProcessId]            INT              NULL,
    [BuyerComment]            NVARCHAR (MAX)   NULL,
    [BuyerCommentDate]        DATETIME         NULL,
    [CreatedDate]             DATETIME         NULL,
    [CreatedBy]               UNIQUEIDENTIFIER NULL,
    [EditedDate]              DATETIME         NULL,
    [EditedBy]                UNIQUEIDENTIFIER NULL,
    [IsActive]                BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_EmbellishmentApproval] PRIMARY KEY CLUSTERED ([EmbellishmentApprovalId] ASC),
    CONSTRAINT [FK_Mrc_EmbellishmentApproval_Mrc_ApprovalStatus] FOREIGN KEY ([ApprovalStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Mrc_EmbellishmentApproval_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]),
    CONSTRAINT [FK_Mrc_EmbellishmentApproval_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId])
);

