CREATE TABLE [dbo].[Mrc_TrimsAndAccessoriesApprovalDetail] (
    [TrimsAndAccessoriesApprovalDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [TrimsAndAccessoriesApprovalId]       INT              NOT NULL,
    [TrimsAndAccessoriesId]               INT              NOT NULL,
    [ApprovalStatusId]                    INT              NOT NULL,
    [BuyerComment]                        NVARCHAR (MAX)   NULL,
    [BuyerCommentDate]                    DATETIME         NULL,
    [PlannedStartDate]                    DATETIME         NULL,
    [PlannedEndDate]                      DATETIME         NULL,
    [ActualStartDate]                     DATETIME         NULL,
    [ActualEndDate]                       DATETIME         NULL,
    [CreatedDate]                         DATETIME         NULL,
    [CreatedBy]                           UNIQUEIDENTIFIER NULL,
    [EditedDate]                          DATETIME         NULL,
    [EditedBy]                            UNIQUEIDENTIFIER NULL,
    [IsActive]                            BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_TrimsAndAccessoriesApprovalDetail] PRIMARY KEY CLUSTERED ([TrimsAndAccessoriesApprovalDetailId] ASC),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesApprovalDetail_Mrc_TrimsAndAccessories] FOREIGN KEY ([ApprovalStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesApprovalDetail_Mrc_TrimsAndAccessories1] FOREIGN KEY ([TrimsAndAccessoriesId]) REFERENCES [dbo].[Mrc_TrimsAndAccessories] ([TrimsAndAccessoriesId]),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesApprovalDetail_Mrc_TrimsAndAccessoriesApproval] FOREIGN KEY ([TrimsAndAccessoriesApprovalId]) REFERENCES [dbo].[Mrc_TrimsAndAccessoriesApproval] ([TrimsAndAccessoriesApprovalId])
);

