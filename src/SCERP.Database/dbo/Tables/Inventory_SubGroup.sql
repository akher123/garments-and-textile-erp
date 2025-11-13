CREATE TABLE [dbo].[Inventory_SubGroup] (
    [SubGroupId]   INT              IDENTITY (1, 1) NOT NULL,
    [SubGroupName] NVARCHAR (100)   NOT NULL,
    [SubGroupCode] NVARCHAR (100)   NOT NULL,
    [GroupId]      INT              NOT NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              CONSTRAINT [DF_InvSubGroup_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_InvSubGroup] PRIMARY KEY CLUSTERED ([SubGroupId] ASC),
    CONSTRAINT [FK_Inventory_SubGroup_Inventory_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Inventory_Group] ([GroupId])
);

