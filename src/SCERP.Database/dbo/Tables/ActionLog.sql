CREATE TABLE [dbo].[ActionLog] (
    [ActionLogId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [LogDate]       DATETIME         NOT NULL,
    [FormName]      VARCHAR (150)    NOT NULL,
    [ActivityName]  VARCHAR (150)    NOT NULL,
    [Action]        VARCHAR (50)     NOT NULL,
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [ActivityRefId] VARCHAR (50)     NOT NULL,
    [CompId]        VARCHAR (3)      NOT NULL,
    CONSTRAINT [PK_ActionLog] PRIMARY KEY CLUSTERED ([ActionLogId] ASC)
);

