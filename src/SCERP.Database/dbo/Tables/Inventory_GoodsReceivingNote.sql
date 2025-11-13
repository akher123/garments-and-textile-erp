CREATE TABLE [dbo].[Inventory_GoodsReceivingNote] (
    [GoodsReceivingNotesId] INT              IDENTITY (1, 1) NOT NULL,
    [GRNNumber]             NVARCHAR (100)   NOT NULL,
    [QualityCertificateId]  INT              NOT NULL,
    [GRNDate]               DATETIME         NOT NULL,
    [IsSendToStoreLedger]   BIT              NOT NULL,
    [Remarks]               NVARCHAR (MAX)   NULL,
    [Description]           NVARCHAR (MAX)   NULL,
    [CreatedDate]           DATETIME         NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    [EditedDate]            DATETIME         NULL,
    [EditedBy]              UNIQUEIDENTIFIER NULL,
    [IsActive]              BIT              CONSTRAINT [DF_GoodsReceivingNote_IsActive] DEFAULT ((1)) NOT NULL,
    [DeductionAmt]          NUMERIC (18, 5)  NULL,
    [IsApproved]            BIT              NULL,
    [AppBy]                 UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_GoodsReceivingNote] PRIMARY KEY CLUSTERED ([GoodsReceivingNotesId] ASC),
    CONSTRAINT [FK_Inventory_GoodsReceivingNote_Inventory_QualityCertificate] FOREIGN KEY ([QualityCertificateId]) REFERENCES [dbo].[Inventory_QualityCertificate] ([QualityCertificateId])
);

