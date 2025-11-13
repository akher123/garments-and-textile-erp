CREATE TABLE [dbo].[TrackMachineLog] (
    [MachineLogId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [MachineLogRefId] NVARCHAR (10)    NOT NULL,
    [MachineId]       INT              NOT NULL,
    [CompanyId]       NVARCHAR (3)     NOT NULL,
    [MachineActionId] INT              NOT NULL,
    [EmployeeId]      UNIQUEIDENTIFIER NULL,
    [ActionDate]      DATETIME         NULL,
    [Remarks]         NVARCHAR (MAX)   NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [CreatedDate]     DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_TrackMachineLog] PRIMARY KEY CLUSTERED ([MachineLogId] ASC)
);

