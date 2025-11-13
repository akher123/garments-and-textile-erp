CREATE TABLE [dbo].[Inventory_QualityCertificate] (
    [QualityCertificateId] INT              IDENTITY (1, 1) NOT NULL,
    [ItemStoreId]          BIGINT           NOT NULL,
    [QCReferenceNo]        NVARCHAR (100)   NOT NULL,
    [IsGrnConverted]       BIT              NULL,
    [SendingDate]          DATETIME         NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              CONSTRAINT [DF_QualityCertificate_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_QualityCertificate] PRIMARY KEY CLUSTERED ([QualityCertificateId] ASC),
    CONSTRAINT [FK_Inventory_QualityCertificate_Inventory_ItemStore] FOREIGN KEY ([ItemStoreId]) REFERENCES [dbo].[Inventory_ItemStore] ([ItemStoreId])
);

