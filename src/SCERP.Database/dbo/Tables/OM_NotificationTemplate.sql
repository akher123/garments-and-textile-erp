CREATE TABLE [dbo].[OM_NotificationTemplate] (
    [NotificationTemplateId] INT           IDENTITY (1, 1) NOT NULL,
    [CompId]                 CHAR (3)      NOT NULL,
    [BuyerRefId]             CHAR (3)      NOT NULL,
    [ActivityId]             INT           NOT NULL,
    [BeforeDays]             FLOAT (53)    NOT NULL,
    [Receiver]               VARCHAR (50)  NULL,
    [Remarks]                VARCHAR (150) NULL,
    CONSTRAINT [PK_OM_NotificationTemplate] PRIMARY KEY CLUSTERED ([NotificationTemplateId] ASC),
    CONSTRAINT [FK_OM_NotificationTemplate_OM_TnaActivity] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[OM_TnaActivity] ([ActivityId])
);

