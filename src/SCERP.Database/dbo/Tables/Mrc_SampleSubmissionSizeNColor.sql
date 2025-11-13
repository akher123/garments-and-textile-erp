CREATE TABLE [dbo].[Mrc_SampleSubmissionSizeNColor] (
    [SampleSubmissionSizeNColorId] INT              IDENTITY (1, 1) NOT NULL,
    [SizeId]                       INT              NULL,
    [ColorId]                      INT              NULL,
    [SampleSubmissionId]           INT              NOT NULL,
    [SampleSubmissionStatusId]     INT              NULL,
    [PlannedStartDate]             DATETIME         NULL,
    [PlannedEndDate]               DATETIME         NULL,
    [ActualStartDate]              DATETIME         NULL,
    [ActualEndDate]                DATETIME         NULL,
    [CreatedDate]                  DATETIME         NULL,
    [CreatedBy]                    UNIQUEIDENTIFIER NULL,
    [EditedDate]                   DATETIME         NULL,
    [EditedBy]                     UNIQUEIDENTIFIER NULL,
    [IsActive]                     BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SampleSubmissionSizeNColor] PRIMARY KEY CLUSTERED ([SampleSubmissionSizeNColorId] ASC),
    CONSTRAINT [FK_Mrc_SampleSubmissionSizeNColor_Mrc_SampleSubmission] FOREIGN KEY ([SampleSubmissionId]) REFERENCES [dbo].[Mrc_SampleSubmission] ([SampleSubmissionId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleSubmissionSizeNColor_Mrc_SendingStatus] FOREIGN KEY ([SampleSubmissionStatusId]) REFERENCES [dbo].[Mrc_SendingStatus] ([SendingStatusId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleSubmissionSizeNColor_Mrc_StyleColor] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Mrc_StyleColor] ([StyleColorId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleSubmissionSizeNColor_Mrc_StyleSize] FOREIGN KEY ([SizeId]) REFERENCES [dbo].[Mrc_StyleSize] ([StyleSizeId]) ON DELETE CASCADE
);

