CREATE TABLE [dbo].[Inventory_RequsitionType] (
    [RequisitionTypeId] INT              IDENTITY (1, 1) NOT NULL,
    [Title]             NVARCHAR (100)   NOT NULL,
    [Description]       NVARCHAR (MAX)   NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              CONSTRAINT [DF_Inventory_RequsitionType_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_RequsitionType] PRIMARY KEY CLUSTERED ([RequisitionTypeId] ASC)
);

