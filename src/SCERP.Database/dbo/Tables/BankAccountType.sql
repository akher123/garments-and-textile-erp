CREATE TABLE [dbo].[BankAccountType] (
    [BankAccountTypeId] INT              IDENTITY (1, 1) NOT NULL,
    [AccountType]       NVARCHAR (100)   NOT NULL,
    [Description]       NVARCHAR (MAX)   NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_BankAccountType] PRIMARY KEY CLUSTERED ([BankAccountTypeId] ASC)
);

