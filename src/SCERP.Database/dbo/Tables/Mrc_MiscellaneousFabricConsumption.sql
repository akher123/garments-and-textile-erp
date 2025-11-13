CREATE TABLE [dbo].[Mrc_MiscellaneousFabricConsumption] (
    [FabricConsumptionId]        INT             IDENTITY (1, 1) NOT NULL,
    [Length]                     NUMERIC (18, 3) NOT NULL,
    [Width]                      NUMERIC (18, 3) NOT NULL,
    [Allowance]                  NUMERIC (18, 3) NOT NULL,
    [GSM]                        NUMERIC (18, 3) NOT NULL,
    [Wastage]                    NUMERIC (18, 3) NULL,
    [NumberOfPcs]                INT             NULL,
    [MiscellaneousFabConsumtion] NUMERIC (18, 3) NULL,
    CONSTRAINT [PK_Mrc_MiscellaneousFabricConsumtion] PRIMARY KEY CLUSTERED ([FabricConsumptionId] ASC),
    CONSTRAINT [FK_Mrc_MiscellaneousFabricConsumtion_Mrc_FabricConsumtion] FOREIGN KEY ([FabricConsumptionId]) REFERENCES [dbo].[Mrc_FabricConsumption] ([FabricConsumptionId])
);

