CREATE TABLE [dbo].[Mrc_EmbellishmentApprovalDetail] (
    [EmbellishmentApprovalDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [EmbellishmentApprovalId]       INT              NOT NULL,
    [EmbellishmentId]               INT              NOT NULL,
    [ApprovalStatusId]              INT              NOT NULL,
    [PlannedStartDate]              DATETIME         NULL,
    [PlannedEndDate]                DATETIME         NULL,
    [ActualStartDate]               DATETIME         NULL,
    [ActualEndDate]                 DATETIME         NULL,
    [CreatedDate]                   DATETIME         NULL,
    [CreatedBy]                     UNIQUEIDENTIFIER NULL,
    [EditedDate]                    DATETIME         NULL,
    [EditedBy]                      UNIQUEIDENTIFIER NULL,
    [IsActive]                      BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_EmbellishmentApprovalDetail] PRIMARY KEY CLUSTERED ([EmbellishmentApprovalDetailId] ASC),
    CONSTRAINT [FK_Mrc_EmbellishmentApprovalDetail_Mrc_ApprovalStatus] FOREIGN KEY ([ApprovalStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Mrc_EmbellishmentApprovalDetail_Mrc_Embellishment] FOREIGN KEY ([EmbellishmentId]) REFERENCES [dbo].[Mrc_Embellishment] ([EmbellishmentId]),
    CONSTRAINT [FK_Mrc_EmbellishmentApprovalDetail_Mrc_EmbellishmentApproval] FOREIGN KEY ([EmbellishmentApprovalId]) REFERENCES [dbo].[Mrc_EmbellishmentApproval] ([EmbellishmentApprovalId])
);

