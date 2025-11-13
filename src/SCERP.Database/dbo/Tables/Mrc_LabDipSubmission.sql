CREATE TABLE [dbo].[Mrc_LabDipSubmission] (
    [LabDipSubmissionId]       INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]              INT              NOT NULL,
    [LabDipSubmissionStatusId] INT              NULL,
    [KeyProcessId]             INT              NOT NULL,
    [CurrencyId]               INT              NULL,
    [SubmissionPrice]          DECIMAL (18, 3)  NULL,
    [NumberOfOption]           INT              NULL,
    [CreatedDate]              DATETIME         NULL,
    [CreatedBy]                UNIQUEIDENTIFIER NULL,
    [EditedDate]               DATETIME         NULL,
    [EditedBy]                 UNIQUEIDENTIFIER NULL,
    [IsActive]                 BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_LabDipSubmission] PRIMARY KEY CLUSTERED ([LabDipSubmissionId] ASC),
    CONSTRAINT [FK_Mrc_LabDipSubmission_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currency] ([CurrencyId]),
    CONSTRAINT [FK_Mrc_LabDipSubmission_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]),
    CONSTRAINT [FK_Mrc_LabDipSubmission_Mrc_SendingStatus] FOREIGN KEY ([LabDipSubmissionStatusId]) REFERENCES [dbo].[Mrc_SendingStatus] ([SendingStatusId]),
    CONSTRAINT [FK_Mrc_LabDipSubmission_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId])
);

