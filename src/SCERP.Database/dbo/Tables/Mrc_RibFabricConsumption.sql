CREATE TABLE [dbo].[Mrc_RibFabricConsumption] (
    [FabricConsumptionId] INT             NOT NULL,
    [NeckWidth]           NUMERIC (18, 3) NOT NULL,
    [FrontNeckDrop]       NUMERIC (18, 3) NOT NULL,
    [RibHeight]           NUMERIC (18, 3) NOT NULL,
    [Allowance]           NUMERIC (18, 3) NOT NULL,
    [GSM]                 NUMERIC (18, 3) NOT NULL,
    [Wastage]             NUMERIC (18, 3) NULL,
    [RibFabConsumption]   NUMERIC (18, 3) NULL,
    CONSTRAINT [PK_Mrc_RibFabricConsumtion] PRIMARY KEY CLUSTERED ([FabricConsumptionId] ASC),
    CONSTRAINT [FK_Mrc_RibFabricConsumtion_Mrc_FabricConsumtion] FOREIGN KEY ([FabricConsumptionId]) REFERENCES [dbo].[Mrc_FabricConsumption] ([FabricConsumptionId])
);

