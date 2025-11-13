CREATE TABLE [dbo].[Currency] (
    [CurrencyId]    INT              IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (100)   NOT NULL,
    [NameInBengali] NVARCHAR (100)   NOT NULL,
    [CurrencyCode]  NVARCHAR (10)    NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_Currency] PRIMARY KEY CLUSTERED ([CurrencyId] ASC)
);

