CREATE TABLE [dbo].[SkillMatrix] (
    [SkillMatrixId]  INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]     UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId] NVARCHAR (10)    NOT NULL,
    [Performance]    FLOAT (53)       NULL,
    [Attitude]       FLOAT (53)       NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_SkillMatrix] PRIMARY KEY CLUSTERED ([SkillMatrixId] ASC)
);

