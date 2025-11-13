CREATE TABLE [dbo].[OM_PortOfLoading] (
    [PortOfLoadingId]    INT           IDENTITY (1, 1) NOT NULL,
    [CompId]             VARCHAR (3)   NULL,
    [PortOfLoadingRefId] VARCHAR (2)   NULL,
    [PortOfLoadingName]  NVARCHAR (50) NULL,
    [CountryId]          INT           NULL,
    [SourceId]           INT           NULL,
    [PortType]           VARCHAR (1)   NULL,
    CONSTRAINT [PK_OM_PortOfLoading] PRIMARY KEY CLUSTERED ([PortOfLoadingId] ASC),
    CONSTRAINT [FK_OM_PortOfLoading_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id])
);

