CREATE TABLE [dbo].[Mrc_TrimsAndAccessories] (
    [TrimsAndAccessoriesId]     INT              IDENTITY (1, 1) NOT NULL,
    [TrimsAndAccessoriesTypeId] INT              NOT NULL,
    [Name]                      NVARCHAR (50)    NOT NULL,
    [Description]               NVARCHAR (MAX)   NULL,
    [CreatedDate]               DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [EditedDate]                DATETIME         NULL,
    [EditedBy]                  UNIQUEIDENTIFIER NULL,
    [IsActive]                  BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_TrimAndAccessories] PRIMARY KEY CLUSTERED ([TrimsAndAccessoriesId] ASC),
    CONSTRAINT [FK_Mrc_TrimsAndAccessories_Mrc_TrimsAndAccessoriesType] FOREIGN KEY ([TrimsAndAccessoriesTypeId]) REFERENCES [dbo].[Mrc_TrimsAndAccessoriesType] ([TrimsAndAccessoriesTypeId])
);

