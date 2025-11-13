CREATE TABLE [dbo].[PoliceStation] (
    [Id]            INT              NOT NULL,
    [Name]          NVARCHAR (100)   NOT NULL,
    [NameInBengali] NVARCHAR (100)   NOT NULL,
    [DistrictId]    INT              NOT NULL,
    [CountryId]     INT              NOT NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_PoliceStation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PoliceStation_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PoliceStation_District] FOREIGN KEY ([DistrictId]) REFERENCES [dbo].[District] ([Id])
);

