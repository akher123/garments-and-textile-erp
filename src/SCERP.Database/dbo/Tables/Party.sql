CREATE TABLE [dbo].[Party] (
    [PartyId]           BIGINT           IDENTITY (1, 1) NOT NULL,
    [CompId]            VARCHAR (3)      NULL,
    [Name]              NVARCHAR (100)   NOT NULL,
    [Address]           NVARCHAR (250)   NULL,
    [Email]             VARCHAR (50)     NULL,
    [Phone]             VARCHAR (20)     NULL,
    [ContactPersonName] VARCHAR (50)     NULL,
    [ContactPhone]      VARCHAR (20)     NULL,
    [PartyRefNo]        NVARCHAR (8)     NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              CONSTRAINT [DF_Party_IsActive] DEFAULT ((1)) NOT NULL,
    [PType]             CHAR (1)         NULL,
    [KglId]             INT              NULL,
    [DglId]             INT              NULL,
    [KRglId]            INT              NULL,
    [PGlId]             INT              NULL,
    [EmGlId]            INT              NULL,
    CONSTRAINT [PK_Party] PRIMARY KEY CLUSTERED ([PartyId] ASC)
);

