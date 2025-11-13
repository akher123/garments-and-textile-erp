CREATE TABLE [dbo].[Mrc_LabDipSubmissionHistory] (
    [LabDipSubmissionHistoryId] INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]               INT              NOT NULL,
    [LabDipSubmissionStatusId]  INT              NULL,
    [CurrencyId]                INT              NULL,
    [KeyProcessId]              INT              NOT NULL,
    [PlannedStartDate]          DATETIME         NULL,
    [PlannedEndDate]            DATETIME         NULL,
    [ActualStartDate]           DATETIME         NULL,
    [ActualEndDate]             DATETIME         NULL,
    [SubmissionPrice]           DECIMAL (18, 3)  NULL,
    [NumberOfOption]            INT              NULL,
    [CreatedDate]               DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [EditedDate]                DATETIME         NULL,
    [EditedBy]                  UNIQUEIDENTIFIER NULL,
    [IsActive]                  BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_LabDipSubmissionHistory] PRIMARY KEY CLUSTERED ([LabDipSubmissionHistoryId] ASC),
    CONSTRAINT [FK_Mrc_LabDipSubmissionHistory_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currency] ([CurrencyId]),
    CONSTRAINT [FK_Mrc_LabDipSubmissionHistory_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]),
    CONSTRAINT [FK_Mrc_LabDipSubmissionHistory_Mrc_SendingStatus] FOREIGN KEY ([LabDipSubmissionStatusId]) REFERENCES [dbo].[Mrc_SendingStatus] ([SendingStatusId]),
    CONSTRAINT [FK_Mrc_LabDipSubmissionHistory_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId])
);

