CREATE TABLE [dbo].[Inventory_Item] (
    [ItemId]             INT              IDENTITY (1, 1) NOT NULL,
    [CompId]             VARCHAR (3)      NULL,
    [ItemName]           NVARCHAR (100)   NOT NULL,
    [ItemCode]           NVARCHAR (100)   NOT NULL,
    [SubGroupId]         INT              NOT NULL,
    [MeasurementUinitId] INT              NULL,
    [ReorderLevel]       DECIMAL (18, 2)  NULL,
    [ItemType]           INT              NULL,
    [GenericNameId]      INT              NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              CONSTRAINT [DF_InvItem_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_InvItem] PRIMARY KEY CLUSTERED ([ItemId] ASC),
    CONSTRAINT [FK_Inventory_Item_Inventory_SubGroup] FOREIGN KEY ([SubGroupId]) REFERENCES [dbo].[Inventory_SubGroup] ([SubGroupId])
);

