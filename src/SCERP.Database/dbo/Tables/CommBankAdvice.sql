CREATE TABLE [dbo].[CommBankAdvice] (
    [BankAdviceId] INT              IDENTITY (1, 1) NOT NULL,
    [ExportId]     BIGINT           NULL,
    [AccHeadId]    INT              NULL,
    [HeadType]     NVARCHAR (10)    NULL,
    [Currency]     INT              NULL,
    [Amount]       DECIMAL (18, 2)  NULL,
    [Rate]         DECIMAL (18, 4)  NULL,
    [AmountInTaka] DECIMAL (18, 4)  NULL,
    [Particulars]  NVARCHAR (100)   NULL,
    [BankRefNo]    NVARCHAR (100)   NULL,
    [CompId]       NVARCHAR (3)     NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_CommBankAdvice] PRIMARY KEY CLUSTERED ([BankAdviceId] ASC),
    CONSTRAINT [FK_CommBankAdvice_CommAccHead] FOREIGN KEY ([AccHeadId]) REFERENCES [dbo].[CommAccHead] ([AccHeadId])
);

