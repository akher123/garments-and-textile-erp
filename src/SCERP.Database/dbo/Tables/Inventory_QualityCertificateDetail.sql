CREATE TABLE [dbo].[Inventory_QualityCertificateDetail] (
    [QualityCertificateDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [QualityCertificateId]       INT              NOT NULL,
    [ItemId]                     INT              NOT NULL,
    [CorrectQuantity]            DECIMAL (18, 2)  NOT NULL,
    [RejectedQuantity]           DECIMAL (18, 2)  NOT NULL,
    [Remarks]                    NVARCHAR (MAX)   NULL,
    [CreatedDate]                DATETIME         NULL,
    [CreatedBy]                  UNIQUEIDENTIFIER NULL,
    [EditedDate]                 DATETIME         NULL,
    [EditedBy]                   UNIQUEIDENTIFIER NULL,
    [IsActive]                   BIT              CONSTRAINT [DF_QualityCertificateDetailId_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_QualityCertificateDetailId] PRIMARY KEY CLUSTERED ([QualityCertificateDetailId] ASC),
    CONSTRAINT [FK_Inventory_QualityCertificateDetailId_Inventory_Item] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Inventory_Item] ([ItemId]),
    CONSTRAINT [FK_Inventory_QualityCertificateDetailId_Inventory_QualityCertificate] FOREIGN KEY ([QualityCertificateId]) REFERENCES [dbo].[Inventory_QualityCertificate] ([QualityCertificateId])
);

