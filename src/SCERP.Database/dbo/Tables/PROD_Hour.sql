CREATE TABLE [dbo].[PROD_Hour] (
    [HourId]    INT           IDENTITY (1, 1) NOT NULL,
    [HourRefId] VARCHAR (2)   NOT NULL,
    [HourName]  VARCHAR (100) NULL,
    [Status]    CHAR (1)      NOT NULL,
    [CompId]    VARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_PROD_Hour] PRIMARY KEY CLUSTERED ([HourId] ASC)
);

