CREATE TABLE [dbo].[City] (
    [CityId]    INT           IDENTITY (1, 1) NOT NULL,
    [CityName]  VARCHAR (100) NOT NULL,
    [CountryId] INT           NOT NULL,
    [StateId]   INT           NULL,
    [Latitude]  FLOAT (53)    NULL,
    [Longitude] FLOAT (53)    NULL,
    CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([CityId] ASC),
    CONSTRAINT [FK_City_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id]),
    CONSTRAINT [FK_City_State] FOREIGN KEY ([StateId]) REFERENCES [dbo].[State] ([StateId])
);

