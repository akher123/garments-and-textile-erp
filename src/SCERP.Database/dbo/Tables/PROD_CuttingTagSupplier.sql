CREATE TABLE [dbo].[PROD_CuttingTagSupplier] (
    [CuttingTagSupplierId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [CuttingTagId]         BIGINT           NOT NULL,
    [CompId]               VARCHAR (3)      NOT NULL,
    [PartyId]              BIGINT           NOT NULL,
    [EmblishmentStatus]    INT              NULL,
    [Rate]                 FLOAT (53)       NULL,
    [DeductionAllowance]   FLOAT (53)       NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [CreatedDate]          DATE             NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATE             NULL,
    CONSTRAINT [PK_PROD_CuttingTagSupplier] PRIMARY KEY CLUSTERED ([CuttingTagSupplierId] ASC),
    CONSTRAINT [FK_PROD_CuttingTagSupplier_Party] FOREIGN KEY ([PartyId]) REFERENCES [dbo].[Party] ([PartyId]),
    CONSTRAINT [FK_PROD_CuttingTagSupplier_PROD_CuttingTagSupplier] FOREIGN KEY ([CuttingTagSupplierId]) REFERENCES [dbo].[PROD_CuttingTagSupplier] ([CuttingTagSupplierId])
);

