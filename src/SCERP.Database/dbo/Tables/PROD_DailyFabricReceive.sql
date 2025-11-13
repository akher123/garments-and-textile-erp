CREATE TABLE [dbo].[PROD_DailyFabricReceive] (
    [FabricReceiveId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [CompId]          VARCHAR (3)      NOT NULL,
    [OrderStyleRefId] VARCHAR (7)      NOT NULL,
    [ConsRefId]       VARCHAR (10)     NOT NULL,
    [ColorRefId]      VARCHAR (4)      NOT NULL,
    [ComponentRefId]  VARCHAR (3)      NOT NULL,
    [FabricQty]       DECIMAL (18, 5)  NOT NULL,
    [ReceivedDate]    DATETIME         NOT NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [CreatedDate]     DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    CONSTRAINT [PK_PROD_DailyFabricReceive] PRIMARY KEY CLUSTERED ([FabricReceiveId] ASC)
);

