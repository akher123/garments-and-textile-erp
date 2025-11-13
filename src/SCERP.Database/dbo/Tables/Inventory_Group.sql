CREATE TABLE [dbo].[Inventory_Group] (
    [GroupId]     INT              IDENTITY (1, 1) NOT NULL,
    [GroupName]   NVARCHAR (100)   NOT NULL,
    [GroupCode]   NVARCHAR (100)   NOT NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              CONSTRAINT [DF_InvGroup_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_InvGroup] PRIMARY KEY CLUSTERED ([GroupId] ASC)
);

