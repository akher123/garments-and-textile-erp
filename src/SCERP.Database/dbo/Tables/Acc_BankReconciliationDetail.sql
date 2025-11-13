CREATE TABLE [dbo].[Acc_BankReconciliationDetail] (
    [Id]        INT              IDENTITY (1, 1) NOT NULL,
    [RefId]     INT              NULL,
    [VoucherId] BIGINT           NULL,
    [CDT]       DATETIME         NULL,
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [EDT]       DATETIME         NULL,
    [EditedBy]  UNIQUEIDENTIFIER NULL,
    [IsActive]  BIT              NULL,
    CONSTRAINT [PK_Acc_BankReconciliationDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Acc_BankReconciliationDetail_Acc_BankReconcilationMaster] FOREIGN KEY ([RefId]) REFERENCES [dbo].[Acc_BankReconcilationMaster] ([Id]),
    CONSTRAINT [FK_Acc_BankReconciliationDetail_Acc_VoucherMaster] FOREIGN KEY ([VoucherId]) REFERENCES [dbo].[Acc_VoucherMaster] ([Id])
);

