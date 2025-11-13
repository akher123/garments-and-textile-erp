CREATE TABLE [dbo].[QuitType] (
    [QuitTypeId]    INT              IDENTITY (1, 1) NOT NULL,
    [Type]          NVARCHAR (100)   NOT NULL,
    [TypeInBengali] NVARCHAR (100)   NOT NULL,
    [Description]   NVARCHAR (MAX)   NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_QuitType] PRIMARY KEY CLUSTERED ([QuitTypeId] ASC)
);

