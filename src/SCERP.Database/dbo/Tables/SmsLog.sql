CREATE TABLE [dbo].[SmsLog] (
    [SmsLogId]      INT           IDENTITY (1, 1) NOT NULL,
    [SendDateTime]  DATETIME      NOT NULL,
    [Receiver]      VARCHAR (30)  NOT NULL,
    [Message]       VARCHAR (200) NOT NULL,
    [SendingStatus] CHAR (1)      NOT NULL,
    CONSTRAINT [PK_SmsLog] PRIMARY KEY CLUSTERED ([SmsLogId] ASC)
);


GO
CREATE TRIGGER [dbo].[insert_SmsLog]
ON [dbo].[SmsLog]
AFTER INSERT
AS
DECLARE @Receiver varchar(30)
DECLARE @Message varchar(200)

SELECT @Receiver = Receiver from inserted
SELECT @Message = Message FROM inserted

EXEC [dbo].[send_sms] @Receiver , @Message

--EXEC [dbo].[send_sms]  '01776042772', 'Test'