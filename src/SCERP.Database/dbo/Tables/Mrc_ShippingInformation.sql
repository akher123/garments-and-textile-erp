CREATE TABLE [dbo].[Mrc_ShippingInformation] (
    [ShippingInformationId] INT              IDENTITY (1, 1) NOT NULL,
    [OrderId]               INT              NOT NULL,
    [ShipmentAddress]       NVARCHAR (MAX)   NOT NULL,
    [Transport]             NVARCHAR (100)   NULL,
    [ShipmentTNC]           NVARCHAR (100)   NULL,
    [CreatedDate]           DATETIME         NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    [EditedDate]            DATETIME         NULL,
    [EditedBy]              UNIQUEIDENTIFIER NULL,
    [IsActive]              BIT              NOT NULL,
    CONSTRAINT [PK_ShippingInformation] PRIMARY KEY CLUSTERED ([ShippingInformationId] ASC),
    CONSTRAINT [FK_Mrc_ShippingInformation_Mrc_OrderInformation] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Mrc_OrderInformation] ([OrderId])
);

