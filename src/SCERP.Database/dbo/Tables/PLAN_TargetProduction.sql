CREATE TABLE [dbo].[PLAN_TargetProduction] (
    [TargetProductionId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [CompId]                VARCHAR (3)      NOT NULL,
    [TargetProductionRefId] VARCHAR (10)     NOT NULL,
    [BuyerRefId]            VARCHAR (3)      NOT NULL,
    [OrderNo]               VARCHAR (12)     NOT NULL,
    [OrderStyleRefId]       VARCHAR (7)      NOT NULL,
    [LineId]                INT              NOT NULL,
    [TotalTargetQty]        INT              NOT NULL,
    [StartDate]             DATE             NULL,
    [EndDate]               DATE             NULL,
    [Remarks]               NVARCHAR (100)   NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    [EditedBy]              UNIQUEIDENTIFIER NULL,
    [CreatedDate]           DATETIME         NULL,
    [EditedDate]            DATETIME         NULL,
    CONSTRAINT [PK_PLAN_TargetProduction] PRIMARY KEY CLUSTERED ([TargetProductionId] ASC),
    CONSTRAINT [FK_PLAN_TargetProduction_PLAN_TargetProduction] FOREIGN KEY ([LineId]) REFERENCES [dbo].[Production_Machine] ([MachineId])
);

