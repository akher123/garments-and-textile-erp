CREATE TABLE [dbo].[Mrc_SampleApprovalSizeNColor] (
    [SampleApprovalSizeNColorId] INT              IDENTITY (1, 1) NOT NULL,
    [SizeId]                     INT              NULL,
    [ColorId]                    INT              NULL,
    [SampleApprovalId]           INT              NOT NULL,
    [SampleApprovedStatusId]     INT              NULL,
    [PlannedStartDate]           DATETIME         NULL,
    [PlannedEndDate]             DATETIME         NULL,
    [ActualStartDate]            DATETIME         NULL,
    [ActualEndDate]              DATETIME         NULL,
    [CreatedDate]                DATETIME         NULL,
    [CreatedBy]                  UNIQUEIDENTIFIER NULL,
    [EditedDate]                 DATETIME         NULL,
    [EditedBy]                   UNIQUEIDENTIFIER NULL,
    [IsActive]                   BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SampleApprovalSizeNColor] PRIMARY KEY CLUSTERED ([SampleApprovalSizeNColorId] ASC),
    CONSTRAINT [FK_Mrc_SampleApprovalSizeNColor_Mrc_ApprovalStatus] FOREIGN KEY ([SampleApprovedStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleApprovalSizeNColor_Mrc_SampleApproval] FOREIGN KEY ([SampleApprovalId]) REFERENCES [dbo].[Mrc_SampleApproval] ([SampleApprovalId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleApprovalSizeNColor_Mrc_StyleColor] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Mrc_StyleColor] ([StyleColorId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleApprovalSizeNColor_Mrc_StyleSize] FOREIGN KEY ([SizeId]) REFERENCES [dbo].[Mrc_StyleSize] ([StyleSizeId]) ON DELETE CASCADE
);

