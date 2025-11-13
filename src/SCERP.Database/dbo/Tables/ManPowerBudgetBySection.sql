CREATE TABLE [dbo].[ManPowerBudgetBySection] (
    [ManPowerBudgetBySectionId] INT              IDENTITY (1, 1) NOT NULL,
    [SectionName]               NVARCHAR (100)   NULL,
    [SectionId]                 INT              NULL,
    [BudgetEmployee]            INT              NULL,
    [CreatedDate]               DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [EditedDate]                DATETIME         NULL,
    [EditedBy]                  UNIQUEIDENTIFIER NULL,
    [IsActive]                  BIT              NULL,
    CONSTRAINT [PK_ManPowerBudgetBySection] PRIMARY KEY CLUSTERED ([ManPowerBudgetBySectionId] ASC)
);

