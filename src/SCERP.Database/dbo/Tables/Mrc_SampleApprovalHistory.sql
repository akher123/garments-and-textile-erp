CREATE TABLE [dbo].[Mrc_SampleApprovalHistory] (
    [SampleApprovalHistoryId] INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]             INT              NOT NULL,
    [SampleTypeId]            INT              NOT NULL,
    [SampleApprovedStatusId]  INT              NULL,
    [PlannedStartDate]        DATETIME         NULL,
    [PlannedEndDate]          DATETIME         NULL,
    [ActualStartDate]         DATETIME         NULL,
    [ActualEndDate]           DATETIME         NULL,
    [KeyProcessId]            INT              NULL,
    [BuyerComment]            NVARCHAR (MAX)   NULL,
    [BuyerCommentDate]        DATETIME         NULL,
    [CreatedDate]             DATETIME         NULL,
    [CreatedBy]               UNIQUEIDENTIFIER NULL,
    [EditedDate]              DATETIME         NULL,
    [EditedBy]                UNIQUEIDENTIFIER NULL,
    [IsActive]                BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SampleApprovalHistory] PRIMARY KEY CLUSTERED ([SampleApprovalHistoryId] ASC),
    CONSTRAINT [FK_Mrc_SampleApprovalHistory_Mrc_ApprovalStatus] FOREIGN KEY ([SampleApprovedStatusId]) REFERENCES [dbo].[Mrc_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Mrc_SampleApprovalHistory_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleApprovalHistory_Mrc_SampleType] FOREIGN KEY ([SampleTypeId]) REFERENCES [dbo].[Mrc_SampleType] ([SampleTypeId]),
    CONSTRAINT [FK_Mrc_SampleApprovalHistory_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId]) ON DELETE CASCADE
);

