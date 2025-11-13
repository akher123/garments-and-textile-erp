CREATE TABLE [dbo].[Mrc_LabDipDevelopmentDetail] (
    [LabDipDevelopmentDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [ColorId]                   INT              NULL,
    [LabDipDevelopmentId]       INT              NOT NULL,
    [PantoneNumber]             NVARCHAR (50)    NOT NULL,
    [ReferenceNumber]           NVARCHAR (50)    NULL,
    [CreatedDate]               DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [EditedDate]                DATETIME         NULL,
    [EditedBy]                  UNIQUEIDENTIFIER NULL,
    [IsActive]                  BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_LabDipDevelopmentGradeNColor] PRIMARY KEY CLUSTERED ([LabDipDevelopmentDetailId] ASC),
    CONSTRAINT [FK_Mrc_LabDipDevelopmentDetail_Mrc_LabDipDevelopment] FOREIGN KEY ([LabDipDevelopmentId]) REFERENCES [dbo].[Mrc_LabDipDevelopment] ([LabDipDevelopmentId]),
    CONSTRAINT [FK_Mrc_LabDipDevelopmentDetail_Mrc_StyleColor] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Mrc_StyleColor] ([StyleColorId])
);

