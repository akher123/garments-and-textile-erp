CREATE TABLE [dbo].[Mrc_LabDipApprovalDetail] (
    [LabDipApprovalDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [ColorId]                INT              NOT NULL,
    [LabDipApprovalId]       INT              NOT NULL,
    [LabDipApprovalStatusId] INT              NULL,
    [LabDipOptionId]         INT              NULL,
    [PantoneNumber]          NVARCHAR (50)    NOT NULL,
    [ReferenceNumber]        NVARCHAR (50)    NOT NULL,
    [BuyerComment]           NVARCHAR (MAX)   NULL,
    [BuyerCommentDate]       DATETIME         NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_LabDipApprovalGradeNColor] PRIMARY KEY CLUSTERED ([LabDipApprovalDetailId] ASC),
    CONSTRAINT [FK_Mrc_LabDipApprovalDetail_Mrc_ApprovalStatus] FOREIGN KEY ([LabDipApprovalStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Mrc_LabDipApprovalDetail_Mrc_LabDipApproval] FOREIGN KEY ([LabDipApprovalId]) REFERENCES [dbo].[Mrc_LabDipApproval] ([LabDipApprovalId]),
    CONSTRAINT [FK_Mrc_LabDipApprovalDetail_Mrc_LabDipOption] FOREIGN KEY ([LabDipOptionId]) REFERENCES [dbo].[Mrc_LabDipOption] ([LabDipOptionId]),
    CONSTRAINT [FK_Mrc_LabDipApprovalDetail_Mrc_StyleColor] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Mrc_StyleColor] ([StyleColorId])
);

