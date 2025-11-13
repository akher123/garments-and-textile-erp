CREATE TABLE [dbo].[MIS_UserActivity] (
    [UserActivityId] INT           IDENTITY (1, 1) NOT NULL,
    [ModuleName]     VARCHAR (150) NOT NULL,
    [ActivityName]   VARCHAR (150) NOT NULL,
    [ToDayData]      INT           NOT NULL,
    [MonthlyData]    INT           NOT NULL,
    [YearlyData]     INT           NOT NULL,
    [SlNo]           INT           NOT NULL,
    [CompId]         VARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_MIS_UserActivity] PRIMARY KEY CLUSTERED ([UserActivityId] ASC)
);

