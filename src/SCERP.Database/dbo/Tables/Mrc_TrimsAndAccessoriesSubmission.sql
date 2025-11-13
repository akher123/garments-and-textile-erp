CREATE TABLE [dbo].[Mrc_TrimsAndAccessoriesSubmission] (
    [TrimsAndAccessoriesSubmissionId] INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]                     INT              NOT NULL,
    [KeyProcessId]                    INT              NOT NULL,
    [SendingStatusId]                 INT              NOT NULL,
    [CurrencyId]                      INT              NULL,
    [SubmissionPrice]                 NUMERIC (18, 3)  NULL,
    [CreatedDate]                     DATETIME         NULL,
    [CreatedBy]                       UNIQUEIDENTIFIER NULL,
    [EditedDate]                      DATETIME         NULL,
    [EditedBy]                        UNIQUEIDENTIFIER NULL,
    [IsActive]                        BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_TrimsAndAccessoriesSubmission] PRIMARY KEY CLUSTERED ([TrimsAndAccessoriesSubmissionId] ASC),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesSubmission_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currency] ([CurrencyId]),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesSubmission_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesSubmission_Mrc_SendingStatus] FOREIGN KEY ([SendingStatusId]) REFERENCES [dbo].[Mrc_SendingStatus] ([SendingStatusId]),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesSubmission_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId])
);

