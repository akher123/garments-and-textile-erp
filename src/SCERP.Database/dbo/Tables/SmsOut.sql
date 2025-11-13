CREATE TABLE [dbo].[SmsOut] (
    [SmsOutId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Receiver] VARCHAR (50)  NOT NULL,
    [Message]  VARCHAR (150) NOT NULL,
    [XStatus]  CHAR (1)      NOT NULL,
    CONSTRAINT [PK_SmsOut] PRIMARY KEY CLUSTERED ([SmsOutId] ASC)
);

