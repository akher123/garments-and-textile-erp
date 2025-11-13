CREATE TABLE [dbo].[Mrc_LabDipSubmissionDetail] (
    [LabDipSubmissionDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [ColorId]                  INT              NULL,
    [LabDipSubmissionId]       INT              NOT NULL,
    [PantoneNumber]            NVARCHAR (50)    NOT NULL,
    [ReferenceNumber]          NVARCHAR (50)    NULL,
    [CreatedDate]              DATETIME         NULL,
    [CreatedBy]                UNIQUEIDENTIFIER NULL,
    [EditedDate]               DATETIME         NULL,
    [EditedBy]                 UNIQUEIDENTIFIER NULL,
    [IsActive]                 BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_LabDipSubmissionGradeNColor] PRIMARY KEY CLUSTERED ([LabDipSubmissionDetailId] ASC),
    CONSTRAINT [FK_Mrc_LabDipSubmissionDetail_Mrc_LabDipSubmission] FOREIGN KEY ([LabDipSubmissionId]) REFERENCES [dbo].[Mrc_LabDipSubmission] ([LabDipSubmissionId]),
    CONSTRAINT [FK_Mrc_LabDipSubmissionDetail_Mrc_StyleColor] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Mrc_StyleColor] ([StyleColorId])
);

