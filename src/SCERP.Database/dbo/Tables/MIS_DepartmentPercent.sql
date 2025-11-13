CREATE TABLE [dbo].[MIS_DepartmentPercent] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [DepartmentId] INT             NULL,
    [Percentage]   DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_MIS_DepartmentPercent] PRIMARY KEY CLUSTERED ([Id] ASC)
);

