CREATE TABLE [dbo].[PROD_DyeingSpChallanDetail] (
    [DyeingSpChallanDetailId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [DyeingSpChallanId]       BIGINT        NOT NULL,
    [BatchId]                 BIGINT        NOT NULL,
    [BatchDetailId]           BIGINT        NOT NULL,
    [SpGroupId]               INT           NOT NULL,
    [GreyWeight]              FLOAT (53)    NOT NULL,
    [FinishWeight]            FLOAT (53)    NULL,
    [Rate]                    FLOAT (53)    NOT NULL,
    [Remarks]                 VARCHAR (150) NULL,
    [CompId]                  VARCHAR (3)   NULL,
    [CcuffQty]                FLOAT (53)    NULL,
    CONSTRAINT [PK_PROD_DyeingSpChallanDetail] PRIMARY KEY CLUSTERED ([DyeingSpChallanDetailId] ASC),
    CONSTRAINT [FK_PROD_DyeingSpChallanDetail_Pro_Batch] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[Pro_Batch] ([BatchId]),
    CONSTRAINT [FK_PROD_DyeingSpChallanDetail_PROD_BatchDetail] FOREIGN KEY ([BatchDetailId]) REFERENCES [dbo].[PROD_BatchDetail] ([BatchDetailId]),
    CONSTRAINT [FK_PROD_DyeingSpChallanDetail_PROD_DyeingSpChallan] FOREIGN KEY ([DyeingSpChallanId]) REFERENCES [dbo].[PROD_DyeingSpChallan] ([DyeingSpChallanId]),
    CONSTRAINT [FK_PROD_DyeingSpChallanDetail_PROD_GroupSubProcess] FOREIGN KEY ([SpGroupId]) REFERENCES [dbo].[PROD_GroupSubProcess] ([GroupSubProcessId])
);

