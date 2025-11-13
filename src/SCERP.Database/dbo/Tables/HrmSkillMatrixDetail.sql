CREATE TABLE [dbo].[HrmSkillMatrixDetail] (
    [SkillMatrixDetailId]  INT              IDENTITY (1, 1) NOT NULL,
    [SkillMatrixId]        INT              NOT NULL,
    [SkillMatrixProcessId] INT              NOT NULL,
    [ProcessPercentage]    INT              NOT NULL,
    [SkillMatrixGradeId]   INT              NOT NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    [CompId]               VARCHAR (3)      NOT NULL,
    CONSTRAINT [PK_PROD_SkillMatrixDetail] PRIMARY KEY CLUSTERED ([SkillMatrixDetailId] ASC),
    CONSTRAINT [FK_HrmSkillMatrixDetail_HrmSkillMatrix] FOREIGN KEY ([SkillMatrixId]) REFERENCES [dbo].[HrmSkillMatrix] ([SkillMatrixId]),
    CONSTRAINT [FK_HrmSkillMatrixDetail_HrmSkillMatrixGrade] FOREIGN KEY ([SkillMatrixGradeId]) REFERENCES [dbo].[HrmSkillMatrixGrade] ([SkillMatrixGradeId]),
    CONSTRAINT [FK_HrmSkillMatrixDetail_HrmSkillMatrixProcess] FOREIGN KEY ([SkillMatrixProcessId]) REFERENCES [dbo].[HrmSkillMatrixProcess] ([SkillMatrixProcessId])
);

