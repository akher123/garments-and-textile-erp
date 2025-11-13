CREATE TABLE [dbo].[PROD_DyeingJobOrderDetail] (
    [DyeingJobOrderDetailId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [DyeingJobOrderId]       BIGINT        NOT NULL,
    [ItemId]                 INT           NOT NULL,
    [ComponentRefId]         VARCHAR (3)   NULL,
    [ColorRefId]             VARCHAR (4)   NULL,
    [MdSizeRefId]            VARCHAR (4)   NULL,
    [FdSizeRefId]            VARCHAR (4)   NULL,
    [Quantity]               FLOAT (53)    NOT NULL,
    [Rate]                   FLOAT (53)    NOT NULL,
    [CompId]                 VARCHAR (3)   NOT NULL,
    [Remarks]                VARCHAR (150) NULL,
    [Gsm]                    FLOAT (53)    NULL,
    [GreyWit]                FLOAT (53)    NULL,
    CONSTRAINT [PK_PROD_DyeingJobOrderDetail] PRIMARY KEY CLUSTERED ([DyeingJobOrderDetailId] ASC),
    CONSTRAINT [FK_PROD_DyeingJobOrderDetail_PROD_DyeingJobOrder] FOREIGN KEY ([DyeingJobOrderId]) REFERENCES [dbo].[PROD_DyeingJobOrder] ([DyeingJobOrderId])
);

