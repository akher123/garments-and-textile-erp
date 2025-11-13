CREATE TABLE [dbo].[Acc_VoucherToCostcentre] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [VoucherNo]    BIGINT           NOT NULL,
    [VoucherRefNo] NVARCHAR (50)    NULL,
    [CostCentreId] INT              NOT NULL,
    [AccountCode]  NUMERIC (18)     NOT NULL,
    [Amount]       NUMERIC (18, 2)  NOT NULL,
    [Date]         DATETIME         NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_Acc_VoucherSegregationToCosCentre] PRIMARY KEY CLUSTERED ([Id] ASC)
);

