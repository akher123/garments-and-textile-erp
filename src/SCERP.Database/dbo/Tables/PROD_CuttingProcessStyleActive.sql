CREATE TABLE [dbo].[PROD_CuttingProcessStyleActive] (
    [CuttingProcessStyleActiveId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [ProcessRefId]                NVARCHAR (3) NOT NULL,
    [CompId]                      VARCHAR (3)  NOT NULL,
    [BuyerRefId]                  VARCHAR (3)  NOT NULL,
    [OrderNo]                     VARCHAR (12) NOT NULL,
    [OrderStyleRefId]             VARCHAR (7)  NOT NULL,
    [StartDate]                   DATETIME     NOT NULL,
    [EndDate]                     DATETIME     NULL,
    CONSTRAINT [PK_PROD_CuttingProcessStyleActive] PRIMARY KEY CLUSTERED ([CuttingProcessStyleActiveId] ASC)
);

