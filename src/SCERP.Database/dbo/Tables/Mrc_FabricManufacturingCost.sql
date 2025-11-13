CREATE TABLE [dbo].[Mrc_FabricManufacturingCost] (
    [FabricManufacturingCostId] INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]               INT              NOT NULL,
    [Description]               NVARCHAR (MAX)   NULL,
    [Particular]                NVARCHAR (100)   NULL,
    [PricePerKg]                NUMERIC (18, 3)  NOT NULL,
    [NetConsPerDzn]             NUMERIC (18, 3)  NOT NULL,
    [TotalPrice]                NUMERIC (18, 3)  NULL,
    [PricePerPcs]               NUMERIC (18, 3)  NULL,
    [CreatedDate]               DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [EditedDate]                DATETIME         NULL,
    [EditedBy]                  UNIQUEIDENTIFIER NULL,
    [IsActive]                  BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_FabricManufacturingCost] PRIMARY KEY CLUSTERED ([FabricManufacturingCostId] ASC),
    CONSTRAINT [FK_Mrc_FabricManufacturingCost_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId]) ON DELETE CASCADE
);

