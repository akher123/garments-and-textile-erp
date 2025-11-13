CREATE TABLE [dbo].[OM_EmbWorkOrderDetail] (
    [EmbWorkOrderDetailId] INT           IDENTITY (1, 1) NOT NULL,
    [BuyerRefId]           CHAR (3)      NOT NULL,
    [OrderNo]              CHAR (12)     NOT NULL,
    [OrderStyleRefId]      CHAR (7)      NOT NULL,
    [EmbWorkOrderId]       INT           NOT NULL,
    [ItemName]             VARCHAR (150) NOT NULL,
    [GColorRefId]          CHAR (4)      NOT NULL,
    [GSizeRefId]           CHAR (4)      NOT NULL,
    [FabricType]           VARCHAR (100) NULL,
    [EmbellishmentType]    VARCHAR (100) NULL,
    [ComponentRefId]       VARCHAR (3)   NOT NULL,
    [FinishColor]          VARCHAR (100) NULL,
    [FinishSize]           VARCHAR (100) NULL,
    [Qty]                  INT           NOT NULL,
    [Rate]                 FLOAT (53)    NOT NULL,
    [Remarks]              VARCHAR (150) NULL,
    [CompId]               CHAR (3)      NOT NULL,
    CONSTRAINT [PK_OM_EmbWorkOrderDetail] PRIMARY KEY CLUSTERED ([EmbWorkOrderDetailId] ASC),
    CONSTRAINT [FK_OM_EmbWorkOrderDetail_OM_EmbWorkOrder] FOREIGN KEY ([EmbWorkOrderId]) REFERENCES [dbo].[OM_EmbWorkOrder] ([EmbWorkOrderId])
);

