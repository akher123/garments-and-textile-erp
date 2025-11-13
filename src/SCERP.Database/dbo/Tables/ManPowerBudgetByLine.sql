CREATE TABLE [dbo].[ManPowerBudgetByLine] (
    [ManPowerBudgetByLineId] INT              IDENTITY (1, 1) NOT NULL,
    [LineName]               NVARCHAR (100)   NULL,
    [LineId]                 INT              NULL,
    [BudgetEmployee]         INT              NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_ManPowerBudgetByLine] PRIMARY KEY CLUSTERED ([ManPowerBudgetByLineId] ASC)
);

