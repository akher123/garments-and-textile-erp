CREATE TABLE [dbo].[Mrc_BodyFabricConsumption] (
    [FabricConsumptionId] INT             NOT NULL,
    [BodyLength]          NUMERIC (18, 3) NOT NULL,
    [SleeveLength]        NUMERIC (18, 3) NOT NULL,
    [LengthWiseAllowance] NUMERIC (18, 3) NOT NULL,
    [HalfChest]           NUMERIC (18, 3) NOT NULL,
    [WidthWiseAllowance]  NUMERIC (18, 3) NOT NULL,
    [GSM]                 NUMERIC (18, 3) NOT NULL,
    [Wastage]             NUMERIC (18, 3) NULL,
    [BodyFabConsumtion]   NUMERIC (18, 3) NULL,
    CONSTRAINT [PK_Mrc_BodyFabricConsumtion] PRIMARY KEY CLUSTERED ([FabricConsumptionId] ASC),
    CONSTRAINT [FK_Mrc_BodyFabricConsumtion_Mrc_FabricConsumtion] FOREIGN KEY ([FabricConsumptionId]) REFERENCES [dbo].[Mrc_FabricConsumption] ([FabricConsumptionId])
);

