CREATE TABLE [dbo].[EmployeeStatus] (
    [EmployeeStatusId] INT              NOT NULL,
    [Name]             NVARCHAR (100)   NULL,
    [NameInBengali]    NVARCHAR (100)   NULL,
    [CreatedDate]      DATETIME         NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]       DATETIME         NULL,
    [EditedBy]         UNIQUEIDENTIFIER NULL,
    [IsActive]         BIT              NULL,
    CONSTRAINT [PK_EmployeeCategory] PRIMARY KEY CLUSTERED ([EmployeeStatusId] ASC)
);

