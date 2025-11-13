CREATE TABLE [dbo].[Unit] (
    [UnitId]        INT              IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (100)   NOT NULL,
    [NameInBengali] NVARCHAR (100)   NOT NULL,
    [Description]   NVARCHAR (MAX)   NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED ([UnitId] ASC)
);

