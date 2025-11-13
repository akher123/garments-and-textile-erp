CREATE TABLE [dbo].[Mrc_EmbellishmentDevelopmentHistory] (
    [EmbellishmentDevelopmentHistoryId] INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]                       INT              NOT NULL,
    [KeyProcessId]                      INT              NOT NULL,
    [ReadyStatusId]                     INT              NULL,
    [DepartmentId]                      INT              NULL,
    [ResponsiblePersonId]               UNIQUEIDENTIFIER NULL,
    [PlannedStartDate]                  DATETIME         NULL,
    [PlannedEndDate]                    DATETIME         NULL,
    [ActualStartDate]                   DATETIME         NULL,
    [ActualEndDate]                     DATETIME         NULL,
    [CreatedDate]                       DATETIME         NULL,
    [CreatedBy]                         UNIQUEIDENTIFIER NULL,
    [EditedDate]                        DATETIME         NULL,
    [EditedBy]                          UNIQUEIDENTIFIER NULL,
    [IsActive]                          BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_EmbellishmentDevelopmentHistory] PRIMARY KEY CLUSTERED ([EmbellishmentDevelopmentHistoryId] ASC),
    CONSTRAINT [FK_Mrc_EmbellishmentDevelopmentHistory_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Department] ([Id]),
    CONSTRAINT [FK_Mrc_EmbellishmentDevelopmentHistory_Employee] FOREIGN KEY ([ResponsiblePersonId]) REFERENCES [dbo].[Employee] ([EmployeeId]),
    CONSTRAINT [FK_Mrc_EmbellishmentDevelopmentHistory_Mrc_KeyProcess] FOREIGN KEY ([KeyProcessId]) REFERENCES [dbo].[Mrc_KeyProcess] ([KeyProcessId]),
    CONSTRAINT [FK_Mrc_EmbellishmentDevelopmentHistory_Mrc_ReadyStatus] FOREIGN KEY ([ReadyStatusId]) REFERENCES [dbo].[Mrc_ReadyStatus] ([ReadyStatusId]),
    CONSTRAINT [FK_Mrc_EmbellishmentDevelopmentHistory_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId])
);

