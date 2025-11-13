CREATE TABLE [dbo].[OM_FabricOrder] (
    [FabricOrderId]     INT              IDENTITY (1, 1) NOT NULL,
    [FabricOrderRefId]  VARCHAR (7)      NOT NULL,
    [SupplierId]        INT              NOT NULL,
    [OrderDate]         DATETIME         NULL,
    [ExpDate]           DATE             NULL,
    [PreparedBy]        UNIQUEIDENTIFIER NULL,
    [Remarks]           NVARCHAR (MAX)   NOT NULL,
    [CompId]            VARCHAR (3)      NOT NULL,
    [BuyerRefId]        VARCHAR (3)      NULL,
    [OrderNo]           VARCHAR (12)     NULL,
    [MerchandiserRefId] VARCHAR (4)      NULL,
    [Status]            CHAR (1)         NULL,
    CONSTRAINT [PK_OM_FabricOrder] PRIMARY KEY CLUSTERED ([FabricOrderId] ASC),
    CONSTRAINT [FK_OM_FabricOrder_Mrc_SupplierCompany] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Mrc_SupplierCompany] ([SupplierCompanyId])
);

