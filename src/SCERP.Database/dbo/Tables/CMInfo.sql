CREATE TABLE [dbo].[CMInfo] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [Date]              DATETIME        NOT NULL,
    [LineId]            INT             NULL,
    [LineName]          NVARCHAR (100)  NULL,
    [NoOfEmployee]      INT             NULL,
    [TotalWorkingHours] DECIMAL (18, 2) NULL,
    [Salary]            DECIMAL (18, 2) NULL,
    [CostOfMinute]      DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_CMInfo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

