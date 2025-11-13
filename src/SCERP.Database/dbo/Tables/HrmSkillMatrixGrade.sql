CREATE TABLE [dbo].[HrmSkillMatrixGrade] (
    [SkillMatrixGradeId] INT              IDENTITY (1, 1) NOT NULL,
    [GradeName]          NVARCHAR (10)    NOT NULL,
    [GradeValueFrom]     INT              NULL,
    [GradeValueTo]       INT              NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              NOT NULL,
    [CompId]             VARCHAR (3)      NULL,
    CONSTRAINT [PK_HrmSkillMatrixGrade] PRIMARY KEY CLUSTERED ([SkillMatrixGradeId] ASC)
);

