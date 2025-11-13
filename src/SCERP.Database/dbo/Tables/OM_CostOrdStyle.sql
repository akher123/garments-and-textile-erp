CREATE TABLE [dbo].[OM_CostOrdStyle] (
    [CostOrderStyleId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompId]           VARCHAR (3)     NOT NULL,
    [OrderStyleRefId]  VARCHAR (7)     NOT NULL,
    [CostDate]         DATE            NULL,
    [CostRefId]        VARCHAR (4)     NOT NULL,
    [CostRate]         DECIMAL (18, 5) NOT NULL,
    [Qty]              FLOAT (53)      NULL,
    [Unit]             VARCHAR (50)    NULL,
    CONSTRAINT [PK_OM_CostOrdStyle] PRIMARY KEY CLUSTERED ([CostOrderStyleId] ASC)
);

