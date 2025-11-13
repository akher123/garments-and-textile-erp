CREATE TABLE [dbo].[District] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [CountryId]     INT              NOT NULL,
    [Name]          NVARCHAR (100)   NOT NULL,
    [NameInBengali] NVARCHAR (100)   NOT NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_District_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id]) ON DELETE CASCADE
);

