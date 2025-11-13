CREATE TABLE [dbo].[EmployeeDocuments] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]  UNIQUEIDENTIFIER NOT NULL,
    [Title]       NVARCHAR (100)   NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [Path]        NVARCHAR (100)   NOT NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

