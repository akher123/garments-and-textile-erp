CREATE TABLE [dbo].[EmailTemplateUser] (
    [EmailTamplateUserId] INT         IDENTITY (1, 1) NOT NULL,
    [CompId]              VARCHAR (3) NOT NULL,
    [EailTamplateId]      INT         NOT NULL,
    [EmailUserId]         INT         NOT NULL,
    [SendingType]         CHAR (2)    NOT NULL,
    CONSTRAINT [PK_EMailTemplateUser] PRIMARY KEY CLUSTERED ([EmailTamplateUserId] ASC),
    CONSTRAINT [FK_EmailTemplateUser_EmailTemplate] FOREIGN KEY ([EailTamplateId]) REFERENCES [dbo].[EmailTemplate] ([EmailTemplateId]),
    CONSTRAINT [FK_EmailTemplateUser_EmailUser] FOREIGN KEY ([EmailUserId]) REFERENCES [dbo].[EmailUser] ([EmailUserId])
);

