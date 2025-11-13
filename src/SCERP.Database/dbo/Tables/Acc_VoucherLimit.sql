CREATE TABLE [dbo].[Acc_VoucherLimit] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [VoucherType]  NVARCHAR (50)    NULL,
    [VoucherLimit] NUMERIC (18, 2)  NULL,
    [CDT]          DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EDT]          DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NULL,
    CONSTRAINT [PK_Acc_VoucherLimit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

