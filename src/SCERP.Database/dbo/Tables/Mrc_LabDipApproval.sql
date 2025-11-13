CREATE TABLE [dbo].[Mrc_LabDipApproval] (
    [LabDipApprovalId]       INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]            INT              NOT NULL,
    [LabDipApprovedStatusId] INT              NULL,
    [KeyProcessId]           INT              NOT NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_LabDipApproval] PRIMARY KEY CLUSTERED ([LabDipApprovalId] ASC),
    CONSTRAINT [FK_Mrc_LabDipApproval_Mrc_ApprovalStatus] FOREIGN KEY ([LabDipApprovedStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Mrc_LabDipApproval_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]),
    CONSTRAINT [FK_Mrc_LabDipApproval_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId])
);

