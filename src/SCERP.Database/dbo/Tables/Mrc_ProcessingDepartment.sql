CREATE TABLE [dbo].[Mrc_ProcessingDepartment] (
    [ProcessingDepartmentId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]                   NVARCHAR (100) NOT NULL,
    [ProcessKey]             NVARCHAR (100) NULL,
    [CreatedDate]            DATETIME       NULL,
    [EditedDate]             DATETIME       NULL,
    [IsActive]               BIT            NOT NULL,
    CONSTRAINT [PK_Mrc_ProcessingDepartment] PRIMARY KEY CLUSTERED ([ProcessingDepartmentId] ASC)
);

