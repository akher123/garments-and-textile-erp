CREATE TABLE [dbo].[Inventory_Brand] (
    [BrandId]     INT              IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100)   NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [IsActive]    BIT              CONSTRAINT [DF_Inventory_Brand_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_Brand] PRIMARY KEY CLUSTERED ([BrandId] ASC)
);

