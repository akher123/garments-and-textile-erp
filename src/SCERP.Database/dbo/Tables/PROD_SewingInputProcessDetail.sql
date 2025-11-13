CREATE TABLE [dbo].[PROD_SewingInputProcessDetail] (
    [SewingInputProcessDetailId] BIGINT      IDENTITY (1, 1) NOT NULL,
    [SewingInputProcessId]       BIGINT      NOT NULL,
    [SizeRefId]                  VARCHAR (4) NOT NULL,
    [InputQuantity]              INT         NOT NULL,
    [CompId]                     VARCHAR (3) NOT NULL,
    CONSTRAINT [PK_PROD_SewingInputProcessDetail] PRIMARY KEY CLUSTERED ([SewingInputProcessDetailId] ASC),
    CONSTRAINT [FK_PROD_SewingInputProcessDetail_PROD_SewingInputProcess] FOREIGN KEY ([SewingInputProcessId]) REFERENCES [dbo].[PROD_SewingInputProcess] ([SewingInputProcessId])
);

