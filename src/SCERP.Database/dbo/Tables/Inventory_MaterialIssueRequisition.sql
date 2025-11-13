CREATE TABLE [dbo].[Inventory_MaterialIssueRequisition] (
    [MaterialIssueRequisitionId] INT              IDENTITY (1, 1) NOT NULL,
    [IssueReceiveNoteNo]         NVARCHAR (100)   NOT NULL,
    [IssueReceiveNoteDate]       DATETIME         NULL,
    [BranchUnitDepartmentId]     INT              NOT NULL,
    [DepartmentSectionId]        INT              NULL,
    [DepartmentLineId]           INT              NULL,
    [SendingDate]                DATETIME         NULL,
    [PreparedBy]                 UNIQUEIDENTIFIER NOT NULL,
    [SubmittedTo]                UNIQUEIDENTIFIER NOT NULL,
    [IsSentToStore]              BIT              NOT NULL,
    [IsModifiedByStore]          BIT              NOT NULL,
    [Remarks]                    NVARCHAR (MAX)   NULL,
    [CreatedDate]                DATETIME         NULL,
    [CreatedBy]                  UNIQUEIDENTIFIER NULL,
    [EditedDate]                 DATETIME         NULL,
    [EditedBy]                   UNIQUEIDENTIFIER NULL,
    [IsActive]                   BIT              CONSTRAINT [DF_MaterialIssueRequisition_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_MaterialIssueRequisition] PRIMARY KEY CLUSTERED ([MaterialIssueRequisitionId] ASC),
    CONSTRAINT [FK_Inventory_MaterialIssueRequisition_Employee] FOREIGN KEY ([SubmittedTo]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

