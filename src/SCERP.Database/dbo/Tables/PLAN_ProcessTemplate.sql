CREATE TABLE [dbo].[PLAN_ProcessTemplate] (
    [Id]                INT              IDENTITY (1, 1) NOT NULL,
    [StylerefId]        NVARCHAR (4)     NULL,
    [ProcessId]         INT              NULL,
    [LeadTime]          INT              NULL,
    [PlannedStartDate]  DATETIME         NULL,
    [PlannedEndDate]    DATETIME         NULL,
    [ActualStartDate]   DATETIME         NULL,
    [ActrualEndDate]    DATETIME         NULL,
    [ResponsiblePerson] UNIQUEIDENTIFIER NULL,
    [NotifyBeforeDays]  INT              NULL,
    [Remarks]           NVARCHAR (MAX)   NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_PLAN_ProcessTemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PLAN_ProcessTemplate_PLAN_Process] FOREIGN KEY ([ProcessId]) REFERENCES [dbo].[PLAN_Process] ([ProcessId]),
    CONSTRAINT [FK_PLAN_ProcessTemplate_PLAN_ResponsiblePerson] FOREIGN KEY ([ResponsiblePerson]) REFERENCES [dbo].[PLAN_ResponsiblePerson] ([ResponsiblePersonId])
);

