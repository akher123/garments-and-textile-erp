CREATE TABLE [dbo].[Inventory_StorePurchaseRequisitionHistory] (
    [StorePurchaseRequisitionHistoryId] INT              IDENTITY (1, 1) NOT NULL,
    [StorePurchaseRequisitionId]        INT              NOT NULL,
    [SubmittedTo]                       UNIQUEIDENTIFIER NOT NULL,
    [SubmittedBy]                       UNIQUEIDENTIFIER NULL,
    [PreparedBy]                        UNIQUEIDENTIFIER NOT NULL,
    [ApprovalStatusId]                  INT              NOT NULL,
    [ApprovalDate]                      DATETIME         NULL,
    [Remarks]                           NVARCHAR (MAX)   NULL,
    [CreatedDate]                       DATETIME         NULL,
    [CreatedBy]                         UNIQUEIDENTIFIER NULL,
    [EditedBy]                          UNIQUEIDENTIFIER NULL,
    [EditedDate]                        DATETIME         NULL,
    [IsActive]                          BIT              CONSTRAINT [DF_Inventory_StorePurchaseRequisitionHistory_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_StorePurchaseRequisitionHistory] PRIMARY KEY CLUSTERED ([StorePurchaseRequisitionHistoryId] ASC),
    CONSTRAINT [FK_Inventory_StorePurchaseRequisitionHistory_Inventory_ApprovalStatus] FOREIGN KEY ([ApprovalStatusId]) REFERENCES [dbo].[Inventory_ApprovalStatus] ([ApprovalStatusId]),
    CONSTRAINT [FK_Inventory_StorePurchaseRequisitionHistory_Inventory_StorePurchaseRequisitionHistory] FOREIGN KEY ([StorePurchaseRequisitionId]) REFERENCES [dbo].[Inventory_StorePurchaseRequisition] ([StorePurchaseRequisitionId])
);

