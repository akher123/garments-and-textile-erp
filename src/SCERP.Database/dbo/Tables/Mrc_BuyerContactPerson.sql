CREATE TABLE [dbo].[Mrc_BuyerContactPerson] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [BuyerId]     INT              NOT NULL,
    [Name]        NVARCHAR (100)   NOT NULL,
    [Address]     NVARCHAR (100)   NULL,
    [Email]       NVARCHAR (100)   NOT NULL,
    [Phone]       NVARCHAR (100)   NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              NOT NULL,
    CONSTRAINT [PK_BuyerContactPerson] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Mrc_BuyerContactPerson_Mrc_Buyer] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Mrc_Buyer] ([Id])
);

