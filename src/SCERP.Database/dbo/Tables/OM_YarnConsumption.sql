CREATE TABLE [dbo].[OM_YarnConsumption] (
    [YarnConsumptionId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompId]            VARCHAR (3)     NULL,
    [ConsRefId]         VARCHAR (10)    NULL,
    [GrColorRefId]      VARCHAR (4)     NULL,
    [ItemCode]          VARCHAR (8)     NULL,
    [KSizeRefId]        VARCHAR (4)     NULL,
    [KColorRefId]       VARCHAR (4)     NULL,
    [CPercent]          NUMERIC (18, 5) NULL,
    [KQty]              NUMERIC (18, 5) NULL,
    [PLoss]             NUMERIC (18, 5) NULL,
    [DReq]              VARCHAR (1)     NULL,
    [WMtr]              NUMERIC (18, 5) NULL,
    [YCRef]             VARCHAR (10)    NULL,
    [Rate]              NUMERIC (18, 5) NULL,
    [SupplierId]        INT             NULL,
    [PiRefId]           VARCHAR (7)     NULL,
    CONSTRAINT [PK_OMN_ConsumptionYarn] PRIMARY KEY CLUSTERED ([YarnConsumptionId] ASC)
);

