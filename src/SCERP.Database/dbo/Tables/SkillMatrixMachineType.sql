CREATE TABLE [dbo].[SkillMatrixMachineType] (
    [MachineTypeId]   INT              IDENTITY (1, 1) NOT NULL,
    [MachineTypeName] NVARCHAR (50)    NOT NULL,
    [Description]     NVARCHAR (MAX)   NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_SkillMatrixMachineType] PRIMARY KEY CLUSTERED ([MachineTypeId] ASC)
);

