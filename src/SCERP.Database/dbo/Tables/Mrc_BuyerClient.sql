CREATE TABLE [dbo].[Mrc_BuyerClient] (
    [BuyerClientId] INT              IDENTITY (1, 1) NOT NULL,
    [ClientName]    NVARCHAR (100)   NOT NULL,
    [BuyerId]       INT              NOT NULL,
    [Address]       NVARCHAR (MAX)   NULL,
    [TellPhoneNo]   NVARCHAR (50)    NULL,
    [Email]         NVARCHAR (50)    NULL,
    [Fax]           NVARCHAR (50)    NULL,
    [Web]           NVARCHAR (50)    NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_BuyerClient] PRIMARY KEY CLUSTERED ([BuyerClientId] ASC),
    CONSTRAINT [FK_Mrc_BuyerClient_Mrc_Buyer] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Mrc_Buyer] ([Id]) ON DELETE CASCADE
);

