CREATE TABLE [dbo].[Inventory_PurchaseRequisitionApprovalFlowConfiguration] (
    [PurchaseRequisitionApprovalFlowConfigurationId] INT              IDENTITY (1, 1) NOT NULL,
    [TagetPersonId]                                  UNIQUEIDENTIFIER NOT NULL,
    [ApprovalFlowControlCode]                        NVARCHAR (50)    NOT NULL,
    [Description]                                    NVARCHAR (MAX)   NULL,
    [CreatedDate]                                    DATETIME         NULL,
    [CreatedBy]                                      UNIQUEIDENTIFIER NULL,
    [EditedDate]                                     DATETIME         NULL,
    [EditedBy]                                       UNIQUEIDENTIFIER NULL,
    [IsActive]                                       BIT              CONSTRAINT [DF_Inventory_PurchaseRequisitionApprovalFlowConfiguration_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_PurchaseRequisitionApprovalFlowConfiguration] PRIMARY KEY CLUSTERED ([PurchaseRequisitionApprovalFlowConfigurationId] ASC),
    CONSTRAINT [FK_Inventory_PurchaseRequisitionApprovalFlowConfiguration_Employee] FOREIGN KEY ([TagetPersonId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

