CREATE TABLE [dbo].[TrackMachineAction] (
    [MachineActionId]   INT              IDENTITY (1, 1) NOT NULL,
    [MachineActionName] NVARCHAR (50)    NOT NULL,
    [CompanyId]         NVARCHAR (3)     NULL,
    [Remarks]           NVARCHAR (MAX)   NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [CreatedDate]       DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_TrackMachineAction] PRIMARY KEY CLUSTERED ([MachineActionId] ASC)
);

