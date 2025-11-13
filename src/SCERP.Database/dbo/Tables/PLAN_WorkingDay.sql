CREATE TABLE [dbo].[PLAN_WorkingDay] (
    [WorkingDayId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompId]       VARCHAR (3)    NOT NULL,
    [WorkingDate]  DATE           NOT NULL,
    [Remarks]      NVARCHAR (100) NOT NULL,
    [DayStatus]    INT            NOT NULL,
    [IsActive]     BIT            NOT NULL,
    CONSTRAINT [PK_PLAN_WorkingDay] PRIMARY KEY CLUSTERED ([WorkingDayId] ASC)
);

