CREATE TABLE [dbo].[PROD_DyeingJobOrder] (
    [DyeingJobOrderId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [JobRefId]         CHAR (6)      NOT NULL,
    [JobDate]          DATETIME      NOT NULL,
    [WorkOrderNo]      VARCHAR (50)  NOT NULL,
    [PartyId]          BIGINT        NOT NULL,
    [DeliveryDate]     DATETIME      NOT NULL,
    [JobType]          CHAR (2)      NOT NULL,
    [BuyerRefId]       VARCHAR (3)   NULL,
    [OrderNo]          VARCHAR (12)  NULL,
    [OrderStyleRefId]  VARCHAR (7)   NULL,
    [BuyerName]        VARCHAR (100) NULL,
    [OrderName]        VARCHAR (50)  NULL,
    [StyleName]        VARCHAR (50)  NULL,
    [Remarks]          VARCHAR (150) NULL,
    [CompId]           VARCHAR (3)   NOT NULL,
    [ProcessRefId]     VARCHAR (3)   NULL,
    CONSTRAINT [PK_PROD_DyeingJobOrder] PRIMARY KEY CLUSTERED ([DyeingJobOrderId] ASC),
    CONSTRAINT [FK_PROD_DyeingJobOrder_Party] FOREIGN KEY ([PartyId]) REFERENCES [dbo].[Party] ([PartyId])
);

