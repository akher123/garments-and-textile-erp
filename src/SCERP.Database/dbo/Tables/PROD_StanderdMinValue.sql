CREATE TABLE [dbo].[PROD_StanderdMinValue] (
    [StanderdMinValueId]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [StanderdMinValueRefId] VARCHAR (7)     NOT NULL,
    [CompId]                VARCHAR (3)     NOT NULL,
    [OrderStyleRefId]       VARCHAR (7)     NOT NULL,
    [StMv]                  NUMERIC (18, 2) NULL,
    CONSTRAINT [PK_PROD_StanderdMinValue] PRIMARY KEY CLUSTERED ([StanderdMinValueId] ASC)
);

