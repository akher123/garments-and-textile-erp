CREATE TABLE [dbo].[HolidaysSetup] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [StartDate]   DATETIME       NOT NULL,
    [EndDate]     DATETIME       NULL,
    [Title]       NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_HolidaysSetup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

