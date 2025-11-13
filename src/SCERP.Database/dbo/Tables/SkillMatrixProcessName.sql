CREATE TABLE [dbo].[SkillMatrixProcessName] (
    [ProcessId]          INT              IDENTITY (1, 1) NOT NULL,
    [ProcessName]        NVARCHAR (50)    NOT NULL,
    [ProcessDescription] NVARCHAR (MAX)   NULL,
    [StandardProcessSmv] FLOAT (53)       NULL,
    [MachineTypeId]      INT              NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              NOT NULL,
    CONSTRAINT [PK_SkillMatrixProcessName] PRIMARY KEY CLUSTERED ([ProcessId] ASC)
);

