CREATE TABLE [dbo].[PLAN_CapacityUtilization] (
    [Id]         BIGINT      IDENTITY (1, 1) NOT NULL,
    [CompId]     VARCHAR (3) NULL,
    [LineId]     INT         NOT NULL,
    [OutputDate] DATETIME    NULL,
    [UsedHour]   FLOAT (53)  NULL,
    [NoMachine]  INT         NOT NULL,
    [OutputMin]  FLOAT (53)  NULL,
    CONSTRAINT [PK_PLAN_CapacityUtilization] PRIMARY KEY CLUSTERED ([Id] ASC)
);

