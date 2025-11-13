CREATE TABLE [dbo].[SkillMatrixDetail] (
    [SkillMatrixDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [SkillMatrixId]       INT              NOT NULL,
    [MachineTypeId]       INT              NULL,
    [ProcessId]           INT              NULL,
    [ProcessSmv]          FLOAT (53)       NULL,
    [ProcessGrade]        NVARCHAR (10)    NULL,
    [AverageCycle]        FLOAT (53)       NULL,
    [StandardProcessSmv]  FLOAT (53)       NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL,
    CONSTRAINT [PK_SkillMatrixDetail] PRIMARY KEY CLUSTERED ([SkillMatrixDetailId] ASC)
);

