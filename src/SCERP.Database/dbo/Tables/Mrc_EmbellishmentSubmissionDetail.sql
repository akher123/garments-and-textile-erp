CREATE TABLE [dbo].[Mrc_EmbellishmentSubmissionDetail] (
    [EmbellishmentSubmissionDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [EmbellishmentSubmissionId]       INT              NOT NULL,
    [EmbellishmentId]                 INT              NOT NULL,
    [SendingStatusId]                 INT              NULL,
    [CurrencyId]                      INT              NULL,
    [Price]                           NUMERIC (18, 3)  NULL,
    [SendingDate]                     DATETIME         NULL,
    [PlannedStartDate]                DATETIME         NULL,
    [PlannedEndDate]                  DATETIME         NULL,
    [ActualStartDate]                 DATETIME         NULL,
    [ActualEndDate]                   DATETIME         NULL,
    [CreatedDate]                     DATETIME         NULL,
    [CreatedBy]                       UNIQUEIDENTIFIER NULL,
    [EditedDate]                      DATETIME         NULL,
    [EditedBy]                        UNIQUEIDENTIFIER NULL,
    [IsActive]                        BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_EmbellishmentSubmissionDetail] PRIMARY KEY CLUSTERED ([EmbellishmentSubmissionDetailId] ASC),
    CONSTRAINT [FK_Mrc_EmbellishmentSubmissionDetail_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currency] ([CurrencyId]),
    CONSTRAINT [FK_Mrc_EmbellishmentSubmissionDetail_Mrc_Embellishment] FOREIGN KEY ([EmbellishmentId]) REFERENCES [dbo].[Mrc_Embellishment] ([EmbellishmentId]),
    CONSTRAINT [FK_Mrc_EmbellishmentSubmissionDetail_Mrc_EmbellishmentSubmission] FOREIGN KEY ([EmbellishmentSubmissionId]) REFERENCES [dbo].[Mrc_EmbellishmentSubmission] ([EmbellishmentSubmissionId]),
    CONSTRAINT [FK_Mrc_EmbellishmentSubmissionDetail_Mrc_SendingStatus] FOREIGN KEY ([SendingStatusId]) REFERENCES [dbo].[Mrc_SendingStatus] ([SendingStatusId])
);

