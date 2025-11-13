CREATE TABLE [dbo].[Inventory_PurchaseType] (
    [PurchaseTypeId] INT              IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (100)   NOT NULL,
    [Description]    NVARCHAR (MAX)   NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [IsActive]       BIT              CONSTRAINT [DF_Inventory_PurchaseType_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_PurchaseType] PRIMARY KEY CLUSTERED ([PurchaseTypeId] ASC)
);

