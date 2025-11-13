CREATE TABLE [dbo].[HrmSkillMatrix] (
    [SkillMatrixId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]    UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR (150)   NOT NULL,
    [Designation]   NVARCHAR (150)   NOT NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    [CompId]        VARCHAR (3)      NOT NULL,
    CONSTRAINT [PK_PROD_SkillMatrix] PRIMARY KEY CLUSTERED ([SkillMatrixId] ASC)
);

