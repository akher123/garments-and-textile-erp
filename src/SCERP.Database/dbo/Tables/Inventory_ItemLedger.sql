CREATE TABLE [dbo].[Inventory_ItemLedger] (
    [ItemLedgerId] INT              NOT NULL,
    [ItemId]       INT              NOT NULL,
    [Quantity]     DECIMAL (18)     NOT NULL,
    [ReorderLevel] DECIMAL (18)     NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              CONSTRAINT [DF_Inventory_ItemLadeger_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_ItemLadeger] PRIMARY KEY CLUSTERED ([ItemLedgerId] ASC),
    CONSTRAINT [FK_Inventory_ItemLedger_Inventory_Item] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Inventory_Item] ([ItemId])
);

