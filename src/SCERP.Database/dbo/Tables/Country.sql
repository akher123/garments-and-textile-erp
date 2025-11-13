CREATE TABLE [dbo].[Country] (
    [Id]                   INT              IDENTITY (1, 1) NOT NULL,
    [CountryName]          NVARCHAR (100)   NOT NULL,
    [CountryNameInBengali] NVARCHAR (100)   NOT NULL,
    [CountryCode]          NVARCHAR (10)    NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([Id] ASC)
);

