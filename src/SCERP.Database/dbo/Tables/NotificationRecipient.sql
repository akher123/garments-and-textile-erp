CREATE TABLE [dbo].[NotificationRecipient] (
    [NotificationRecipientId] INT              IDENTITY (1, 1) NOT NULL,
    [TimeAndActionId]         INT              NOT NULL,
    [SendCopyToEmail]         NVARCHAR (100)   NULL,
    [SendCopyToPhone]         NVARCHAR (100)   NULL,
    [IsNotifyByEmail]         BIT              NOT NULL,
    [IsNotifyByPhone]         BIT              NOT NULL,
    [IsNotifyByPortal]        BIT              NOT NULL,
    [CreatedDate]             DATETIME         NULL,
    [CreatedBy]               UNIQUEIDENTIFIER NULL,
    [EditedDate]              DATETIME         NULL,
    [EditedBy]                UNIQUEIDENTIFIER NULL,
    [IsActive]                BIT              NOT NULL,
    CONSTRAINT [PK_NotificationRecipient] PRIMARY KEY CLUSTERED ([NotificationRecipientId] ASC),
    CONSTRAINT [FK_NotificationRecipient_Mrc_TimeAndActionCalendar] FOREIGN KEY ([TimeAndActionId]) REFERENCES [dbo].[Mrc_TimeAndActionCalendar] ([TimeAndActionId]) ON DELETE CASCADE
);

