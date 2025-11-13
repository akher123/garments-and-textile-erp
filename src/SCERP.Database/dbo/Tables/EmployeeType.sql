CREATE TABLE [dbo].[EmployeeType] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (100)   NOT NULL,
    [TitleInBengali] NVARCHAR (100)   NOT NULL,
    [Description]    NVARCHAR (MAX)   NULL,
    [CDT]            DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EDT]            DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

