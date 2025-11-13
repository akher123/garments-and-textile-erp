CREATE TABLE [dbo].[MIS_HourlyAchievement] (
    [HourId]    INT           IDENTITY (1, 1) NOT NULL,
    [HourRefId] VARCHAR (2)   NULL,
    [HourName]  VARCHAR (100) NULL,
    [Status]    CHAR (1)      NULL,
    [CompId]    VARCHAR (3)   NULL,
    [QTQty]     INT           NULL,
    [QAQty]     INT           NULL,
    [XQty]      INT           NULL
);

