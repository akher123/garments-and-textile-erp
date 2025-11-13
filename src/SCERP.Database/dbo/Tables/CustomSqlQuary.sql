CREATE TABLE [dbo].[CustomSqlQuary] (
    [CustomSqlQuaryId] INT              IDENTITY (1, 1) NOT NULL,
    [SqlQuaryRefId]    VARCHAR (8)      NULL,
    [Name]             NVARCHAR (100)   NULL,
    [SqlQuary]         NVARCHAR (MAX)   NULL,
    [Description]      NVARCHAR (MAX)   NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [EditedBy]         UNIQUEIDENTIFIER NULL,
    [CreatedDate]      DATETIME         NULL,
    [EditedDate]       DATETIME         NULL,
    [IsActive]         BIT              NOT NULL,
    CONSTRAINT [PK_CustomSqlQuary] PRIMARY KEY CLUSTERED ([CustomSqlQuaryId] ASC)
);

