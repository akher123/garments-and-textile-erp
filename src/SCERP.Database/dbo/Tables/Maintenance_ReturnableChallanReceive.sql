CREATE TABLE [dbo].[Maintenance_ReturnableChallanReceive] (
    [ReturnableChallanReceiveId]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [ReturnableChallanDetailId]        BIGINT       NOT NULL,
    [ReturnableChallanReceiveMasterId] BIGINT       NULL,
    [ReceiveDate]                      DATETIME     NULL,
    [ReceiveQty]                       FLOAT (53)   NOT NULL,
    [CompId]                           VARCHAR (3)  NOT NULL,
    [RejectQty]                        FLOAT (53)   NULL,
    [ChallanNo]                        VARCHAR (15) NULL,
    [Amount]                           FLOAT (53)   NULL,
    CONSTRAINT [PK_Maintenance_ReturnableChallanReceive] PRIMARY KEY CLUSTERED ([ReturnableChallanReceiveId] ASC),
    CONSTRAINT [FK_Maintenance_ReturnableChallanReceive_Maintenance_ReturnableChallanDetail] FOREIGN KEY ([ReturnableChallanDetailId]) REFERENCES [dbo].[Maintenance_ReturnableChallanDetail] ([ReturnableChallanDetailId])
);

