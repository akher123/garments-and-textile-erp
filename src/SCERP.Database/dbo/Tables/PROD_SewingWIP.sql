CREATE TABLE [dbo].[PROD_SewingWIP] (
    [CompId]      VARCHAR (3)     NULL,
    [LineId]      INT             NOT NULL,
    [LineName]    NVARCHAR (100)  NULL,
    [OpeningQty]  INT             NULL,
    [InputQty]    INT             NULL,
    [OutputQty]   INT             NULL,
    [WIP]         DECIMAL (18, 2) NULL,
    [Hour]        DECIMAL (18, 2) NULL,
    [RBuyerName]  NVARCHAR (100)  NULL,
    [ROrderName]  VARCHAR (30)    NULL,
    [RStyleName]  VARCHAR (100)   NULL,
    [RColorName]  NVARCHAR (50)   NULL,
    [UCBuyerName] NVARCHAR (100)  NULL,
    [UCOrderName] VARCHAR (30)    NULL,
    [UCStyleName] VARCHAR (100)   NULL,
    [UCColorName] NVARCHAR (50)   NULL
);

