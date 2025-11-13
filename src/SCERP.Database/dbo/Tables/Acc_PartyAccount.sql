CREATE TABLE [dbo].[Acc_PartyAccount] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [PartyId]     BIGINT           NOT NULL,
    [GLId]        INT              NOT NULL,
    [PartyType]   CHAR (1)         NOT NULL,
    [Createdby]   UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate] DATE             NOT NULL,
    [EditedDate]  DATE             NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              NOT NULL,
    CONSTRAINT [PK_AccountMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

