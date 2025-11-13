CREATE TABLE [dbo].[Inventory_MaterialIssue] (
    [MaterialIssueId]            INT              IDENTITY (1, 1) NOT NULL,
    [MaterialIssueRequisitionId] INT              NOT NULL,
    [ToppingType]                INT              NULL,
    [IType]                      INT              NULL,
    [BtRefNo]                    NVARCHAR (8)     NULL,
    [IssueReceiveNo]             NVARCHAR (100)   NOT NULL,
    [IssueReceiveDate]           DATETIME         NULL,
    [PreparedByStore]            UNIQUEIDENTIFIER NOT NULL,
    [Note]                       NVARCHAR (MAX)   NULL,
    [SupplierId]                 INT              NULL,
    [CreatedDate]                DATETIME         NULL,
    [CreatedBy]                  UNIQUEIDENTIFIER NULL,
    [EditedDate]                 DATETIME         NULL,
    [EditedBy]                   UNIQUEIDENTIFIER NULL,
    [IsActive]                   BIT              CONSTRAINT [DF_Inventory_MaterialIssue_IsActive] DEFAULT ((1)) NOT NULL,
    [MachineId]                  INT              NULL,
    [Quantity]                   FLOAT (53)       NULL,
    CONSTRAINT [PK_Inventory_MaterialIssue] PRIMARY KEY CLUSTERED ([MaterialIssueId] ASC),
    CONSTRAINT [FK_Inventory_MaterialIssue_Inventory_MaterialIssue] FOREIGN KEY ([PreparedByStore]) REFERENCES [dbo].[Employee] ([EmployeeId]),
    CONSTRAINT [FK_Inventory_MaterialIssue_Inventory_MaterialIssueRequisition] FOREIGN KEY ([MaterialIssueRequisitionId]) REFERENCES [dbo].[Inventory_MaterialIssueRequisition] ([MaterialIssueRequisitionId])
);

