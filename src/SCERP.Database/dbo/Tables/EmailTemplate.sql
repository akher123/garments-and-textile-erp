CREATE TABLE [dbo].[EmailTemplate] (
    [EmailTemplateId]    INT           IDENTITY (1, 1) NOT NULL,
    [EmailTemplateRefId] VARCHAR (50)  NOT NULL,
    [EmailTemplateName]  VARCHAR (MAX) NULL,
    CONSTRAINT [PK_EMailTemplate] PRIMARY KEY CLUSTERED ([EmailTemplateId] ASC)
);

