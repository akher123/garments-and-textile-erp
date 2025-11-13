CREATE TABLE [dbo].[PROD_StanderdMinValDetail] (
    [StanderdMinValDetailId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [StanderdMinValueId]     BIGINT          NOT NULL,
    [CompId]                 VARCHAR (3)     NOT NULL,
    [StanderdMinValueRefId]  VARCHAR (7)     NOT NULL,
    [SubProcessRefId]        VARCHAR (3)     NOT NULL,
    [StMvD]                  NUMERIC (18, 2) NULL,
    CONSTRAINT [PK_PROD_StanderdMinValDetail_1] PRIMARY KEY CLUSTERED ([StanderdMinValDetailId] ASC),
    CONSTRAINT [FK_PROD_StanderdMinValDetail_PROD_StanderdMinValue] FOREIGN KEY ([StanderdMinValueId]) REFERENCES [dbo].[PROD_StanderdMinValue] ([StanderdMinValueId])
);

