CREATE TABLE [dbo].[OM_ConsumptionSupplier] (
    [ConsumptionSupplierId] INT           IDENTITY (1, 1) NOT NULL,
    [ConsumptionId]         BIGINT        NOT NULL,
    [SupplierId]            INT           NOT NULL,
    [Quantity]              FLOAT (53)    NOT NULL,
    [Percentage]            FLOAT (53)    NOT NULL,
    [Rate]                  FLOAT (53)    NOT NULL,
    [Remarks]               VARCHAR (MAX) NULL,
    [CompId]                VARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_OM_ConsumptionSupplier] PRIMARY KEY CLUSTERED ([ConsumptionSupplierId] ASC),
    CONSTRAINT [FK_OM_ConsumptionSupplier_Mrc_SupplierCompany] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Mrc_SupplierCompany] ([SupplierCompanyId]),
    CONSTRAINT [FK_OM_ConsumptionSupplier_OM_Consumption] FOREIGN KEY ([ConsumptionId]) REFERENCES [dbo].[OM_Consumption] ([ConsumptionId])
);

