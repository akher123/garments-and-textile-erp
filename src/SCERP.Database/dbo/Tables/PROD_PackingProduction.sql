CREATE TABLE [dbo].[PROD_PackingProduction] (
    [PackingProductionId]  BIGINT       NOT NULL,
    [PackingRatioMasterId] BIGINT       NOT NULL,
    [CompId]               CHAR (3)     NOT NULL,
    [PackingRefId]         VARCHAR (7)  NOT NULL,
    [ProdDate]             DATE         NULL,
    [CartonQty]            INT          NOT NULL,
    [ProcessRefId]         VARCHAR (3)  NULL,
    [Remarks]              VARCHAR (50) NULL,
    CONSTRAINT [PK_PROD_PackingProduction] PRIMARY KEY CLUSTERED ([PackingProductionId] ASC),
    CONSTRAINT [FK_PROD_PackingProduction_PROD_PackingRatioMaster] FOREIGN KEY ([PackingRatioMasterId]) REFERENCES [dbo].[PROD_PackingRatioMaster] ([PackingRatioMasterId])
);

