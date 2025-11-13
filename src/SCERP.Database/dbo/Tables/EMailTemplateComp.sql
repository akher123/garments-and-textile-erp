CREATE TABLE [dbo].[EMailTemplateComp] (
    [EailTemplateCompId] INT         IDENTITY (1, 1) NOT NULL,
    [CompId]             VARCHAR (3) NOT NULL,
    [EailTemplateId]     INT         NOT NULL,
    CONSTRAINT [PK_EMailTemplateComp] PRIMARY KEY CLUSTERED ([EailTemplateCompId] ASC),
    CONSTRAINT [FK_EMailTemplateComp_EmailTemplate] FOREIGN KEY ([EailTemplateId]) REFERENCES [dbo].[EmailTemplate] ([EmailTemplateId])
);

