CREATE TABLE [dbo].[Acc_ReferenceNo] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [FromDate]    DATETIME      NULL,
    [ToDate]      DATETIME      NULL,
    [VoucherType] NVARCHAR (50) NULL,
    [ReferenceNo] INT           NULL,
    [IsActive]    BIT           NOT NULL,
    CONSTRAINT [PK_Acc_ReferenceNo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

