CREATE TABLE [dbo].[Inv_ReportLoanStatement] (
    [LoanRefNo]       VARCHAR (50)    NULL,
    [TransactionDate] DATE            NULL,
    [Party]           NVARCHAR (50)   NULL,
    [InvoiceNo]       NVARCHAR (50)   NULL,
    [InvoiceDate]     DATE            NULL,
    [ItemId]          INT             NULL,
    [ReceivedQty]     DECIMAL (18, 2) NULL,
    [GevenQty]        DECIMAL (18, 2) NULL,
    [Remarks]         NVARCHAR (50)   NULL
);

