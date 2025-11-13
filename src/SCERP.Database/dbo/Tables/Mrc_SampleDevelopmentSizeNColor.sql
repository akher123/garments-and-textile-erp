CREATE TABLE [dbo].[Mrc_SampleDevelopmentSizeNColor] (
    [SampleDevelopmentSizeNColorId] INT              IDENTITY (1, 1) NOT NULL,
    [SizeId]                        INT              NULL,
    [ColorId]                       INT              NULL,
    [SampleDevelopmentId]           INT              NOT NULL,
    [SampleReadyStatusId]           INT              NULL,
    [PlannedStartDate]              DATETIME         NULL,
    [PlannedEndDate]                DATETIME         NULL,
    [ActualStartDate]               DATETIME         NULL,
    [ActualEndDate]                 DATETIME         NULL,
    [CreatedDate]                   DATETIME         NULL,
    [CreatedBy]                     UNIQUEIDENTIFIER NULL,
    [EditedDate]                    DATETIME         NULL,
    [EditedBy]                      UNIQUEIDENTIFIER NULL,
    [IsActive]                      BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SampleDevelopmentSizeNColor] PRIMARY KEY CLUSTERED ([SampleDevelopmentSizeNColorId] ASC),
    CONSTRAINT [FK_Mrc_SampleDevelopmentSizeNColor_Mrc_SampleDevelopment] FOREIGN KEY ([SampleDevelopmentId]) REFERENCES [dbo].[Mrc_SampleDevelopment] ([SampleDevelopmentId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mrc_SampleDevelopmentSizeNColor_Mrc_StyleColor] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Mrc_StyleColor] ([StyleColorId]),
    CONSTRAINT [FK_Mrc_SampleDevelopmentSizeNColor_Mrc_StyleSize] FOREIGN KEY ([SizeId]) REFERENCES [dbo].[Mrc_StyleSize] ([StyleSizeId])
);

