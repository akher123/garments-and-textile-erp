CREATE TABLE [dbo].[Mrc_LabDipApprovalHistory] (
    [LabDipApprovalHistoryId] INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]             INT              NOT NULL,
    [LabDipApprovalStatusId]  INT              NULL,
    [KeyProcessId]            INT              NOT NULL,
    [PlannedStartDate]        DATETIME         NULL,
    [PlannedEndDate]          DATETIME         NULL,
    [ActualStartDate]         DATETIME         NULL,
    [ActualEndDate]           DATETIME         NULL,
    [CreatedDate]             DATETIME         NULL,
    [CreatedBy]               UNIQUEIDENTIFIER NULL,
    [EditedDate]              DATETIME         NULL,
    [EditedBy]                UNIQUEIDENTIFIER NULL,
    [IsActive]                BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_LabDipApprovalHistory] PRIMARY KEY CLUSTERED ([LabDipApprovalHistoryId] ASC),
    CONSTRAINT [FK_Mrc_LabDipApprovalHistory_Mrc_ApprovalStatus] FOREIGN KEY ([LabDipApprovalStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Mrc_LabDipApprovalHistory_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]),
    CONSTRAINT [FK_Mrc_LabDipApprovalHistory_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId])
);

