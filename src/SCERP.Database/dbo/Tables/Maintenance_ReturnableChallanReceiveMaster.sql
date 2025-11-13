CREATE TABLE [dbo].[Maintenance_ReturnableChallanReceiveMaster] (
    [ReturnableChallanReceiveMasterId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [RetChallanMasterRefId]            VARCHAR (8)  NOT NULL,
    [ReturnableChallanId]              BIGINT       NULL,
    [ChallanNo]                        VARCHAR (15) NOT NULL,
    [ReceiveDate]                      DATETIME     NOT NULL,
    [TotalAmount]                      FLOAT (53)   NOT NULL,
    [CompId]                           VARCHAR (3)  NOT NULL,
    CONSTRAINT [PK_Maintenance_ReturnableChallanReceiveMaster] PRIMARY KEY CLUSTERED ([ReturnableChallanReceiveMasterId] ASC),
    CONSTRAINT [FK_Maintenance_ReturnableChallanReceiveMaster_Maintenance_ReturnableChallan] FOREIGN KEY ([ReturnableChallanId]) REFERENCES [dbo].[Maintenance_ReturnableChallan] ([ReturnableChallanId])
);

