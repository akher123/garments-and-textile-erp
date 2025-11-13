CREATE TABLE [dbo].[EmployeeCategory] (
    [EmployeeCategoryId] INT              NOT NULL,
    [Title]              NVARCHAR (100)   NOT NULL,
    [TitleInBengali]     NVARCHAR (100)   NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeCategory_1] PRIMARY KEY CLUSTERED ([EmployeeCategoryId] ASC)
);

