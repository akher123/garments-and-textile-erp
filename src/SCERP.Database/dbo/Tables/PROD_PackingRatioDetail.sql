CREATE TABLE [dbo].[PROD_PackingRatioDetail] (
    [PackingRatioDetailId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [PackingRatioMasterId] BIGINT        NOT NULL,
    [ColorRefId]           VARCHAR (4)   NOT NULL,
    [SizeRefId]            VARCHAR (4)   NOT NULL,
    [Quantity]             INT           NOT NULL,
    [Remarks]              VARCHAR (150) NULL,
    CONSTRAINT [PK_PROD_PackingRatioDetail] PRIMARY KEY CLUSTERED ([PackingRatioDetailId] ASC),
    CONSTRAINT [FK_PROD_PackingRatioDetail_PROD_PackingRatioMaster] FOREIGN KEY ([PackingRatioMasterId]) REFERENCES [dbo].[PROD_PackingRatioMaster] ([PackingRatioMasterId])
);

