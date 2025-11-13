CREATE TABLE [dbo].[CommBank] (
    [BankId]      INT              IDENTITY (1, 1) NOT NULL,
    [BankName]    NVARCHAR (100)   NULL,
    [BankAddress] NVARCHAR (MAX)   NULL,
    [BankType]    NVARCHAR (50)    NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              NOT NULL,
    CONSTRAINT [PK_CommBank] PRIMARY KEY CLUSTERED ([BankId] ASC)
);

