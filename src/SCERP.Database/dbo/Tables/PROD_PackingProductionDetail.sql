CREATE TABLE [dbo].[PROD_PackingProductionDetail] (
    [PackingProductionDetailId] BIGINT   IDENTITY (1, 1) NOT NULL,
    [PackingProductionId]       BIGINT   NOT NULL,
    [SequanceNo]                CHAR (4) NOT NULL,
    CONSTRAINT [PK_PROD_PackingProductionDetail] PRIMARY KEY CLUSTERED ([PackingProductionDetailId] ASC),
    CONSTRAINT [FK_PROD_PackingProductionDetail_PROD_PackingProduction] FOREIGN KEY ([PackingProductionId]) REFERENCES [dbo].[PROD_PackingProduction] ([PackingProductionId])
);

