CREATE TABLE [dbo].[MailInformation] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [SendingMailAddress] NVARCHAR (100)   NULL,
    [Password]           NVARCHAR (100)   NULL,
    [SmtpClient]         NVARCHAR (100)   NULL,
    [Port]               NVARCHAR (10)    NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              NULL,
    CONSTRAINT [PK_MailInformation] PRIMARY KEY CLUSTERED ([Id] ASC)
);

