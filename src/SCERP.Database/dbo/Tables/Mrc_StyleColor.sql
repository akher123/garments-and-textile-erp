CREATE TABLE [dbo].[Mrc_StyleColor] (
    [StyleColorId] INT              IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (150)   NULL,
    [Code]         NVARCHAR (100)   NULL,
    [Description]  NVARCHAR (MAX)   NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_StyleColor] PRIMARY KEY CLUSTERED ([StyleColorId] ASC)
);

