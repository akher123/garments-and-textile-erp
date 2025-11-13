CREATE TABLE [dbo].[Acc_BankVoucherManual] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [RefId]       INT              NULL,
    [Date]        DATE             NULL,
    [Particulars] NVARCHAR (500)   NULL,
    [CheckNo]     NVARCHAR (50)    NULL,
    [CheckDate]   DATE             NULL,
    [Amount]      NUMERIC (18, 2)  NULL,
    [Type]        NVARCHAR (50)    NULL,
    [CDT]         DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EDT]         DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              NULL,
    CONSTRAINT [PK_Acc_BankVoucherManual] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Acc_BankVoucherManual_Acc_BankReconcilationMaster] FOREIGN KEY ([RefId]) REFERENCES [dbo].[Acc_BankReconcilationMaster] ([Id])
);

