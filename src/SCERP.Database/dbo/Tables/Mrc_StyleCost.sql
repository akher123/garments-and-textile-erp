CREATE TABLE [dbo].[Mrc_StyleCost] (
    [StyleCostId]    INT              IDENTITY (1, 1) NOT NULL,
    [SpecSheetId]    INT              NOT NULL,
    [CostingHeadId]  INT              NOT NULL,
    [Item]           NVARCHAR (100)   NOT NULL,
    [QuantityPerPc]  INT              NULL,
    [QuantityPerDzn] INT              NULL,
    [UnitOfMeasure]  NVARCHAR (100)   NOT NULL,
    [UnitPrice]      NUMERIC (18, 3)  NOT NULL,
    [Amount]         NUMERIC (18, 3)  NOT NULL,
    [Remark]         NVARCHAR (MAX)   NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_StyleCost] PRIMARY KEY CLUSTERED ([StyleCostId] ASC),
    CONSTRAINT [FK_Mrc_StyleCost_Mrc_CostingHead] FOREIGN KEY ([CostingHeadId]) REFERENCES [dbo].[Mrc_CostingHead] ([CostingHeadId]),
    CONSTRAINT [FK_Mrc_StyleCost_Mrc_SpecificationSheet] FOREIGN KEY ([SpecSheetId]) REFERENCES [dbo].[Mrc_SpecificationSheet] ([SpecificationSheetId]) ON DELETE CASCADE
);

