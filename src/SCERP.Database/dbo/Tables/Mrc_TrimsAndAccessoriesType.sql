CREATE TABLE [dbo].[Mrc_TrimsAndAccessoriesType] (
    [TrimsAndAccessoriesTypeId] INT              IDENTITY (1, 1) NOT NULL,
    [Name]                      NVARCHAR (100)   NOT NULL,
    [Description]               NVARCHAR (MAX)   NULL,
    [CreatedDate]               DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [EditedDate]                DATETIME         NULL,
    [EditedBy]                  UNIQUEIDENTIFIER NULL,
    [IsActive]                  BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_TrimsAndAccessoriesType] PRIMARY KEY CLUSTERED ([TrimsAndAccessoriesTypeId] ASC)
);

