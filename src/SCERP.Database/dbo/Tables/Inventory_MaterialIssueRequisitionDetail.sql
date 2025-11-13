CREATE TABLE [dbo].[Inventory_MaterialIssueRequisitionDetail] (
    [MaterialIssueRequisitionDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [MaterialIssueRequisitionId]       INT              NOT NULL,
    [ItemName]                         NVARCHAR (100)   NOT NULL,
    [ItemDescription]                  NVARCHAR (MAX)   NULL,
    [Size]                             NVARCHAR (100)   NULL,
    [Brand]                            NVARCHAR (100)   NULL,
    [Origin]                           NVARCHAR (100)   NULL,
    [Machine]                          NVARCHAR (MAX)   NULL,
    [RequiredQuantity]                 DECIMAL (18, 2)  NOT NULL,
    [MeasurementUnit]                  NVARCHAR (50)    NULL,
    [DesiredDate]                      DATETIME         NULL,
    [FunctionalArea]                   NVARCHAR (MAX)   NOT NULL,
    [Remarks]                          NVARCHAR (MAX)   NULL,
    [CreatedDate]                      DATETIME         NULL,
    [CreatedBy]                        UNIQUEIDENTIFIER NULL,
    [EditedDate]                       DATETIME         NULL,
    [EditedBy]                         UNIQUEIDENTIFIER NULL,
    [IsActive]                         BIT              CONSTRAINT [DF_MaterialIssueRequisitionDetail_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_MaterialIssueRequisitionDetail] PRIMARY KEY CLUSTERED ([MaterialIssueRequisitionDetailId] ASC),
    CONSTRAINT [FK_Inventory_MaterialIssueRequisitionDetail_Inventory_MaterialIssueRequisition] FOREIGN KEY ([MaterialIssueRequisitionId]) REFERENCES [dbo].[Inventory_MaterialIssueRequisition] ([MaterialIssueRequisitionId])
);

