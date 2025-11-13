CREATE TABLE [dbo].[PROD_GroupSubProcess] (
    [GroupSubProcessId] INT           IDENTITY (1, 1) NOT NULL,
    [GroupName]         VARCHAR (MAX) NOT NULL,
    [SpCode]            VARCHAR (50)  NOT NULL,
    [CompId]            VARCHAR (3)   NOT NULL,
    [GroupType]         VARCHAR (2)   NOT NULL,
    CONSTRAINT [PK_PROD_GroupSubProcess] PRIMARY KEY CLUSTERED ([GroupSubProcessId] ASC)
);

