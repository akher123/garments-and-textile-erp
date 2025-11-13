CREATE TABLE [dbo].[PROD_DyeingSpChallan] (
    [DyeingSpChallanId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [DyeingSpChallanRefId] NCHAR (8)        NOT NULL,
    [ChallanNo]            VARCHAR (50)     NOT NULL,
    [ChallanDate]          DATETIME         NOT NULL,
    [ExpDate]              DATETIME         NULL,
    [ParyId]               BIGINT           NOT NULL,
    [Remarks]              VARCHAR (MAX)    NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [ApprovedBy]           UNIQUEIDENTIFIER NULL,
    [CompId]               VARCHAR (3)      NOT NULL,
    [IsApproved]           BIT              NOT NULL,
    CONSTRAINT [PK_PROD_DyeingSpChallan] PRIMARY KEY CLUSTERED ([DyeingSpChallanId] ASC),
    CONSTRAINT [FK_PROD_DyeingSpChallan_Party] FOREIGN KEY ([ParyId]) REFERENCES [dbo].[Party] ([PartyId])
);

