CREATE TABLE [dbo].[Inventory_ApprovalStatus] (
    [ApprovalStatusId] INT              IDENTITY (1, 1) NOT NULL,
    [StatusName]       NVARCHAR (100)   NOT NULL,
    [Description]      NVARCHAR (MAX)   NULL,
    [CreatedDate]      DATETIME         NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]       DATETIME         NULL,
    [EditedBy]         UNIQUEIDENTIFIER NULL,
    [IsActive]         BIT              CONSTRAINT [DF_Inventory_ApprovalStatus_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_ApprovalStatus] PRIMARY KEY CLUSTERED ([ApprovalStatusId] ASC)
);

