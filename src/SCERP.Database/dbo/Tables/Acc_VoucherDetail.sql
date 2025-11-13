CREATE TABLE [dbo].[Acc_VoucherDetail] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [RefId]          BIGINT          NULL,
    [GLID]           INT             NULL,
    [Particulars]    NVARCHAR (500)  NULL,
    [Debit]          NUMERIC (18, 2) NULL,
    [Credit]         NUMERIC (18, 2) NULL,
    [FirstCurValue]  NUMERIC (18, 2) NOT NULL,
    [SecendCurValue] NUMERIC (18, 2) NOT NULL,
    [ThirdCurValue]  NUMERIC (18, 2) NOT NULL,
    [CostCentreId]   NVARCHAR (50)   NULL,
    [IntRefId]       VARCHAR (10)    NULL,
    [IntType]        INT             NULL,
    CONSTRAINT [PK_Acc_VoucherDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Acc_VoucherDetail_Acc_GLAccounts] FOREIGN KEY ([GLID]) REFERENCES [dbo].[Acc_GLAccounts] ([Id]),
    CONSTRAINT [FK_Acc_VoucherDetail_Acc_VoucherMaster] FOREIGN KEY ([RefId]) REFERENCES [dbo].[Acc_VoucherMaster] ([Id])
);

