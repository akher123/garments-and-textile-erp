CREATE TABLE [dbo].[Mrc_TrimsAndAccessoriesApproval] (
    [TrimsAndAccessoriesApprovalId] INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]                   INT              NOT NULL,
    [ApprovalStatusId]              INT              NULL,
    [KeyProcessId]                  INT              NULL,
    [CreatedDate]                   DATETIME         NULL,
    [CreatedBy]                     UNIQUEIDENTIFIER NULL,
    [EditedDate]                    DATETIME         NULL,
    [EditedBy]                      UNIQUEIDENTIFIER NULL,
    [IsActive]                      BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_TrimsAndAccessoriesApproval] PRIMARY KEY CLUSTERED ([TrimsAndAccessoriesApprovalId] ASC),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesApproval_Mrc_ApprovalStatus] FOREIGN KEY ([ApprovalStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesApproval_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesApproval_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId])
);

