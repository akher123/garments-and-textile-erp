CREATE TABLE [dbo].[HrmSkillMatrixProcess] (
    [SkillMatrixProcessId] INT              IDENTITY (1, 1) NOT NULL,
    [ProcessName]          NVARCHAR (50)    NOT NULL,
    [DisplayOrder]         INT              NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    [CompId]               VARCHAR (3)      NULL,
    CONSTRAINT [PK_HrmSkillMatrixProcess] PRIMARY KEY CLUSTERED ([SkillMatrixProcessId] ASC)
);

