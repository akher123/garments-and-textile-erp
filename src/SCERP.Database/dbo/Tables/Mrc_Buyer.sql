CREATE TABLE [dbo].[Mrc_Buyer] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [BuyerName]       NVARCHAR (100)   NOT NULL,
    [Address]         NVARCHAR (100)   NULL,
    [CountryId]       INT              NULL,
    [City]            NVARCHAR (100)   NULL,
    [State]           NVARCHAR (100)   NULL,
    [ZipCode]         NVARCHAR (100)   NULL,
    [PostCode]        NVARCHAR (100)   NULL,
    [Telephone]       NVARCHAR (100)   NULL,
    [Fax]             NVARCHAR (100)   NULL,
    [Email]           NVARCHAR (100)   NULL,
    [Web]             NVARCHAR (100)   NULL,
    [EntrollmentDate] DATETIME         NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_Buyer] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Buyer_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id]) ON DELETE CASCADE
);

