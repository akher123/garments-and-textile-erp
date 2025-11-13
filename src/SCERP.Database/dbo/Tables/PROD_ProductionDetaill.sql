CREATE TABLE [dbo].[PROD_ProductionDetaill] (
    [ProductionDetailId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [ProductionId]       BIGINT          NOT NULL,
    [CompId]             VARCHAR (50)    NULL,
    [ProductionRefId]    VARCHAR (10)    NOT NULL,
    [ItemCode]           NVARCHAR (100)  NOT NULL,
    [ColorRefId]         VARCHAR (4)     NOT NULL,
    [SizeRefId]          VARCHAR (4)     NOT NULL,
    [MeasurementUinitId] INT             NULL,
    [Qty]                NUMERIC (18, 2) NULL,
    CONSTRAINT [PK_PROD_ProductionDtls] PRIMARY KEY CLUSTERED ([ProductionDetailId] ASC),
    CONSTRAINT [FK_PROD_ProductionDetaill_PROD_Production] FOREIGN KEY ([ProductionId]) REFERENCES [dbo].[PROD_Production] ([ProductionId])
);

