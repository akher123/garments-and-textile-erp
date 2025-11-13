CREATE TABLE [dbo].[PLAN_TNA_Template] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [TemplateId]   INT              NOT NULL,
    [ActivityId]   INT              NULL,
    [FromLeadTime] INT              NULL,
    [ToLeadTime]   INT              NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_PLAN_TNA_Template_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

