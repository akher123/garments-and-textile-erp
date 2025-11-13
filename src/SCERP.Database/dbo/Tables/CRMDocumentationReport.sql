CREATE TABLE [dbo].[CRMDocumentationReport] (
    [Id]                INT              IDENTITY (1, 1) NOT NULL,
    [RefNo]             NVARCHAR (100)   NOT NULL,
    [ReportName]        NVARCHAR (200)   NULL,
    [Description]       NVARCHAR (MAX)   NULL,
    [Literature]        NVARCHAR (MAX)   NULL,
    [ModuleId]          INT              NULL,
    [PhotographPath]    NVARCHAR (200)   NULL,
    [ResponsiblePerson] UNIQUEIDENTIFIER NULL,
    [LastUpdateDate]    DATETIME         NULL,
    [LastUpdateBy]      UNIQUEIDENTIFIER NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_DocumentationReport] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CRMDocumentationReport_CRMCollaborator] FOREIGN KEY ([ResponsiblePerson]) REFERENCES [dbo].[CRMCollaborator] ([CollaboratorId]),
    CONSTRAINT [FK_CRMDocumentationReport_CRMCollaborator1] FOREIGN KEY ([LastUpdateBy]) REFERENCES [dbo].[CRMCollaborator] ([CollaboratorId]),
    CONSTRAINT [FK_CRMDocumentationReport_Module] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[Module] ([Id])
);

