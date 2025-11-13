CREATE TABLE [dbo].[Mrc_SampleApproval] (
    [SampleApprovalId]       INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]            INT              NOT NULL,
    [SampleTypeId]           INT              NOT NULL,
    [SampleApprovedStatusId] INT              NULL,
    [KeyProcessId]           INT              NULL,
    [BuyerComment]           NVARCHAR (MAX)   NULL,
    [BuyerCommentDate]       DATETIME         NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SampleApproval] PRIMARY KEY CLUSTERED ([SampleApprovalId] ASC),
    CONSTRAINT [FK_Mrc_SampleApproval_Mrc_ApprovalStatus] FOREIGN KEY ([SampleApprovedStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Mrc_SampleApproval_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleApproval_Mrc_SampleType] FOREIGN KEY ([SampleTypeId]) REFERENCES [dbo].[Mrc_SampleType] ([SampleTypeId]),
    CONSTRAINT [FK_Mrc_SampleApproval_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId]) ON DELETE CASCADE
);

