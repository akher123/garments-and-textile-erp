CREATE TABLE [dbo].[EmailUser] (
    [EmailUserId]    INT           IDENTITY (1, 1) NOT NULL,
    [CompId]         VARCHAR (3)   NOT NULL,
    [EmailUserRefId] VARCHAR (3)   NOT NULL,
    [EmailUserName]  VARCHAR (100) NOT NULL,
    [EmailAddress]   VARCHAR (50)  NOT NULL,
    [Phone]          VARCHAR (50)  NULL,
    CONSTRAINT [PK_EMailUser] PRIMARY KEY CLUSTERED ([EmailUserId] ASC)
);

