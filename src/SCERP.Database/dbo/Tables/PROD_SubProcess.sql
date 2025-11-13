CREATE TABLE [dbo].[PROD_SubProcess] (
    [SubProcessId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompId]          VARCHAR (3)    NOT NULL,
    [SubProcessRefId] VARCHAR (3)    NOT NULL,
    [ProcessRefId]    VARCHAR (3)    NOT NULL,
    [SubProcessName]  NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_PROD_StanderdMinValDetail] PRIMARY KEY CLUSTERED ([SubProcessId] ASC)
);

