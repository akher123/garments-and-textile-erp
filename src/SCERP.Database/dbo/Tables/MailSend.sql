CREATE TABLE [dbo].[MailSend] (
    [MailSendId]  INT            IDENTITY (1, 1) NOT NULL,
    [ModuleId]    INT            NULL,
    [ReportName]  NVARCHAR (100) NULL,
    [FileName]    NVARCHAR (100) NULL,
    [MailSubject] NVARCHAR (100) NULL,
    [MailBody]    NVARCHAR (MAX) NULL,
    [MailAddress] NVARCHAR (100) NULL,
    [PersonName]  NVARCHAR (50)  NULL,
    [MailType]    NVARCHAR (50)  NULL,
    [Profile]     NVARCHAR (50)  NULL,
    [Status]      NVARCHAR (10)  NULL,
    [Date]        DATETIME       NULL,
    [IsActive]    BIT            NULL,
    [CompId]      VARCHAR (3)    NOT NULL,
    CONSTRAINT [PK_MailSend] PRIMARY KEY CLUSTERED ([MailSendId] ASC)
);

