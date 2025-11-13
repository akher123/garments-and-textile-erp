CREATE TABLE [dbo].[Acc_IncomeStatementMgf] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [g1]              NVARCHAR (2)     NULL,
    [g2]              NVARCHAR (2)     NULL,
    [Particulars]     NVARCHAR (100)   NULL,
    [Amount]          DECIMAL (18, 2)  NULL,
    [Percentage]      DECIMAL (18, 2)  NULL,
    [CompanySectorId] INT              NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_Acc_IncomeStatementMgf] PRIMARY KEY CLUSTERED ([Id] ASC)
);

