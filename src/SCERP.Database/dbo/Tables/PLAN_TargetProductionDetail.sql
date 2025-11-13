CREATE TABLE [dbo].[PLAN_TargetProductionDetail] (
    [TragetProductionDetailId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [CompId]                   VARCHAR (3) NOT NULL,
    [TargetProductionId]       BIGINT      NOT NULL,
    [TargetDate]               DATE        NULL,
    [TargetQty]                INT         NOT NULL,
    CONSTRAINT [PK_PLAN_TargetProductionDetail] PRIMARY KEY CLUSTERED ([TragetProductionDetailId] ASC),
    CONSTRAINT [FK_PLAN_TargetProductionDetail_PLAN_TargetProduction] FOREIGN KEY ([TargetProductionId]) REFERENCES [dbo].[PLAN_TargetProduction] ([TargetProductionId])
);

