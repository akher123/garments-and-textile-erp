CREATE TABLE [dbo].[Mrc_SampleSubmission] (
    [SampleSubmissionId]       INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]              INT              NOT NULL,
    [SampleTypeId]             INT              NOT NULL,
    [SampleSubmissionStatusId] INT              NULL,
    [KeyProcessId]             INT              NULL,
    [Quantity]                 INT              NULL,
    [SubmissionPrice]          DECIMAL (18, 3)  NULL,
    [CurrencyId]               INT              NULL,
    [CreatedDate]              DATETIME         NULL,
    [CreatedBy]                UNIQUEIDENTIFIER NULL,
    [EditedDate]               DATETIME         NULL,
    [EditedBy]                 UNIQUEIDENTIFIER NULL,
    [IsActive]                 BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SampleSubmission] PRIMARY KEY CLUSTERED ([SampleSubmissionId] ASC),
    CONSTRAINT [FK_Mrc_SampleSubmission_Mrc_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currency] ([CurrencyId]),
    CONSTRAINT [FK_Mrc_SampleSubmission_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleSubmission_Mrc_SampleType] FOREIGN KEY ([SampleTypeId]) REFERENCES [dbo].[Mrc_SampleType] ([SampleTypeId]),
    CONSTRAINT [FK_Mrc_SampleSubmission_Mrc_SendingStatus] FOREIGN KEY ([SampleSubmissionStatusId]) REFERENCES [dbo].[Mrc_SendingStatus] ([SendingStatusId]),
    CONSTRAINT [FK_Mrc_SampleSubmission_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId]) ON DELETE CASCADE
);

