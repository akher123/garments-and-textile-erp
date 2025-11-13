CREATE TABLE [dbo].[Mrc_TrimsAndAccessoriesDevelopment] (
    [TrimsAndAccessoriesDevelopmentId] INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]                      INT              NOT NULL,
    [KeyProcessId]                     INT              NOT NULL,
    [ReadyStatusId]                    INT              NOT NULL,
    [CreatedDate]                      DATETIME         NULL,
    [CreatedBy]                        UNIQUEIDENTIFIER NULL,
    [EditedDate]                       DATETIME         NULL,
    [EditedBy]                         UNIQUEIDENTIFIER NULL,
    [IsActive]                         BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_TrimAndAccessoriesDevelopment] PRIMARY KEY CLUSTERED ([TrimsAndAccessoriesDevelopmentId] ASC),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesDevelopment_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesDevelopment_Mrc_ReadyStatus] FOREIGN KEY ([ReadyStatusId]) REFERENCES [dbo].[Mrc_ReadyStatus] ([ReadyStatusId]),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesDevelopment_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId])
);

