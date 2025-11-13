CREATE TABLE [dbo].[PLAN_DailyLineLayout] (
    [LineLayoutId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompId]          VARCHAR (3)    NOT NULL,
    [LineId]          INT            NOT NULL,
    [OutputDate]      DATETIME       NOT NULL,
    [NumberOfMachine] INT            NOT NULL,
    [PlanQty]         INT            NOT NULL,
    [Remarks]         NVARCHAR (250) NULL,
    CONSTRAINT [PK_PLAN_DailyLineLayout] PRIMARY KEY CLUSTERED ([LineLayoutId] ASC),
    CONSTRAINT [FK_PLAN_DailyLineLayout_Production_Machine] FOREIGN KEY ([LineId]) REFERENCES [dbo].[Production_Machine] ([MachineId])
);

