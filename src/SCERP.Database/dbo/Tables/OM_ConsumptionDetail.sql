CREATE TABLE [dbo].[OM_ConsumptionDetail] (
    [ConsumptionDetailId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompId]              VARCHAR (3)     NOT NULL,
    [ConsRefId]           VARCHAR (10)    NOT NULL,
    [GColorRefId]         VARCHAR (4)     NULL,
    [GSizeRefId]          VARCHAR (4)     NULL,
    [PColorRefId]         VARCHAR (4)     NULL,
    [PSizeRefId]          VARCHAR (4)     NULL,
    [QuantityP]           NUMERIC (18, 2) NULL,
    [PPQty]               DECIMAL (18, 5) NULL,
    [PAllow]              DECIMAL (18, 5) NULL,
    [TotalQty]            DECIMAL (18, 5) NULL,
    [Remarks]             VARCHAR (150)   NULL,
    CONSTRAINT [PK_OM_ConsumptionDetail] PRIMARY KEY CLUSTERED ([ConsumptionDetailId] ASC)
);

