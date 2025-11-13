CREATE TABLE [dbo].[Inventory_Size] (
    [SizeId]      INT              IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (100)   NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              CONSTRAINT [DF_Inventory_Size_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_Size] PRIMARY KEY CLUSTERED ([SizeId] ASC)
);

