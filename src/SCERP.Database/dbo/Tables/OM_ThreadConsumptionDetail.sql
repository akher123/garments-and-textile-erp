CREATE TABLE [dbo].[OM_ThreadConsumptionDetail] (
    [ThreadConsumptionDetailId] INT           IDENTITY (1, 1) NOT NULL,
    [ThreadConsumptionId]       INT           NOT NULL,
    [ItemName]                  VARCHAR (100) NOT NULL,
    [Color]                     VARCHAR (50)  NOT NULL,
    [Component]                 VARCHAR (50)  NOT NULL,
    [UnitType]                  VARCHAR (50)  NOT NULL,
    [ConsQty]                   FLOAT (53)    NOT NULL,
    [Remarks]                   VARCHAR (150) NULL,
    CONSTRAINT [PK_OM_ThreadConsumptionDetail] PRIMARY KEY CLUSTERED ([ThreadConsumptionDetailId] ASC),
    CONSTRAINT [FK_OM_ThreadConsumptionDetail_OM_ThreadConsumption] FOREIGN KEY ([ThreadConsumptionId]) REFERENCES [dbo].[OM_ThreadConsumption] ([ThreadConsumptionId])
);

