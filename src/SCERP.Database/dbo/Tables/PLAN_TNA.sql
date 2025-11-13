CREATE TABLE [dbo].[PLAN_TNA] (
    [Id]                INT              IDENTITY (1, 1) NOT NULL,
    [CompId]            NVARCHAR (3)     NULL,
    [OrderStyleRefId]   NVARCHAR (7)     NULL,
    [ActivityId]        INT              NULL,
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
    CONSTRAINT [PK_PLANProcess] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PLAN_TNA_PLAN_Activity] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[PLAN_Activity] ([Id]),
    CONSTRAINT [FK_PLAN_TNA_PLAN_ResponsiblePerson] FOREIGN KEY ([ResponsiblePerson]) REFERENCES [dbo].[PLAN_ResponsiblePerson] ([ResponsiblePersonId])
);

