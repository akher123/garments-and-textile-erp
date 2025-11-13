CREATE TABLE [dbo].[OM_FabricOrderDetail] (
    [FabricOrderDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [FabricOrderId]       INT              NOT NULL,
    [OrderStyleRefId]     VARCHAR (7)      NULL,
    [CompId]              VARCHAR (3)      NOT NULL,
    [YLocked]             BIT              NOT NULL,
    [LockedBy]            UNIQUEIDENTIFIER NULL,
    [LockedDate]          DATE             NULL,
    [UnLockedBy]          UNIQUEIDENTIFIER NULL,
    [UnLockedDate]        DATE             NULL,
    CONSTRAINT [PK_OM_FabricOrderDetail] PRIMARY KEY CLUSTERED ([FabricOrderDetailId] ASC),
    CONSTRAINT [FK_OM_FabricOrderDetail_OM_FabricOrder] FOREIGN KEY ([FabricOrderId]) REFERENCES [dbo].[OM_FabricOrder] ([FabricOrderId])
);

