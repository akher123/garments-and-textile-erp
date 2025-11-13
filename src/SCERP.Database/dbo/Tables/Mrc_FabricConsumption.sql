CREATE TABLE [dbo].[Mrc_FabricConsumption] (
    [FabricConsumptionId]  INT              IDENTITY (1, 1) NOT NULL,
    [SpecificationSheetId] INT              NOT NULL,
    [StyleSize]            NVARCHAR (100)   NULL,
    [TotalFabConsumption]  NUMERIC (18, 3)  NULL,
    [MeasurementUnit]      NVARCHAR (100)   NULL,
    [Color]                NVARCHAR (100)   NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_FabricConsumtion] PRIMARY KEY CLUSTERED ([FabricConsumptionId] ASC),
    CONSTRAINT [FK_Mrc_FabricConsumption_Mrc_SpecificationSheet] FOREIGN KEY ([SpecificationSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId]) ON DELETE CASCADE
);

