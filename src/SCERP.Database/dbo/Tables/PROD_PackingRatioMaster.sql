CREATE TABLE [dbo].[PROD_PackingRatioMaster] (
    [PackingRatioMasterId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [PackingRefId]         VARCHAR (7)   NOT NULL,
    [CompId]               CHAR (3)      NOT NULL,
    [OrderShipRefId]       VARCHAR (50)  NOT NULL,
    [PcsQty]               INT           NOT NULL,
    [PackName]             VARCHAR (50)  NULL,
    [Description]          VARCHAR (150) NULL,
    CONSTRAINT [PK_PROD_PackingRatioMaster] PRIMARY KEY CLUSTERED ([PackingRatioMasterId] ASC)
);

