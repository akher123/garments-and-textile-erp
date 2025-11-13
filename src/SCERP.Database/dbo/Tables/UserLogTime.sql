CREATE TABLE [dbo].[UserLogTime] (
    [UserLogTimeId]   UNIQUEIDENTIFIER NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [SessionId]       VARCHAR (50)     NOT NULL,
    [LoginTime]       DATETIME         NOT NULL,
    [LogoutTime]      DATETIME         NULL,
    [UserHostAddress] VARCHAR (MAX)    NULL,
    [BrowserName]     VARCHAR (250)    NOT NULL,
    [BrowserVerssion] VARCHAR (250)    NOT NULL,
    [Offline]         BIT              NOT NULL,
    CONSTRAINT [PK_UserLogTime] PRIMARY KEY CLUSTERED ([UserLogTimeId] ASC)
);

