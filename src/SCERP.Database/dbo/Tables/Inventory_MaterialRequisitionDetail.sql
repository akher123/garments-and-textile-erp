CREATE TABLE [dbo].[Inventory_MaterialRequisitionDetail] (
    [MaterialRequisitionDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [MaterialRequisitionId]       INT              NOT NULL,
    [ItemName]                    NVARCHAR (100)   NOT NULL,
    [ItemDescription]             NVARCHAR (MAX)   NULL,
    [Size]                        NVARCHAR (50)    NULL,
    [Brand]                       NVARCHAR (100)   NULL,
    [Origin]                      NVARCHAR (100)   NULL,
    [RequiredQuantity]            DECIMAL (18, 2)  NOT NULL,
    [MeasurementUnit]             NVARCHAR (50)    NULL,
    [DesiredDate]                 DATETIME         NULL,
    [FunctionalArea]              NVARCHAR (MAX)   NULL,
    [ApprovedQuantity]            DECIMAL (18, 2)  NULL,
    [Remarks]                     NVARCHAR (MAX)   NULL,
    [CreatedDate]                 DATETIME         NULL,
    [CreatedBy]                   UNIQUEIDENTIFIER NULL,
    [EditedDate]                  DATETIME         NULL,
    [EditedBy]                    UNIQUEIDENTIFIER NULL,
    [IsActive]                    BIT              CONSTRAINT [DF_Inventory_MaterialRequisitionDetail_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_MaterialRequisitionDetail] PRIMARY KEY CLUSTERED ([MaterialRequisitionDetailId] ASC),
    CONSTRAINT [FK_Inventory_MaterialRequisitionDetail_Inventory_MaterialRequisition] FOREIGN KEY ([MaterialRequisitionId]) REFERENCES [dbo].[Inventory_MaterialRequisition] ([MaterialRequisitionId])
);

