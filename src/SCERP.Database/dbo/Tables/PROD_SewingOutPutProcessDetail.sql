CREATE TABLE [dbo].[PROD_SewingOutPutProcessDetail] (
    [SewingOutPutProcessDetailId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [SewingOutPutProcessId]       BIGINT      NOT NULL,
    [SizeRefId]                   VARCHAR (4) NOT NULL,
    [Quantity]                    INT         NOT NULL,
    [CompId]                      VARCHAR (3) NOT NULL,
    [QcRejectQty]                 INT         NULL,
    CONSTRAINT [PK_PROD_SewingOutPutDetail] PRIMARY KEY CLUSTERED ([SewingOutPutProcessDetailId] ASC),
    CONSTRAINT [FK_PROD_SewingOutPutProcessDetail_PROD_SewingOutPutProcess] FOREIGN KEY ([SewingOutPutProcessId]) REFERENCES [dbo].[PROD_SewingOutPutProcess] ([SewingOutPutProcessId])
);

