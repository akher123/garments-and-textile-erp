CREATE TABLE [dbo].[Mrc_SampleInvoice] (
    [SampleInvoiceId]    INT              IDENTITY (1, 1) NOT NULL,
    [Title]              NVARCHAR (100)   NULL,
    [Description]        NVARCHAR (MAX)   NULL,
    [FilePath]           NVARCHAR (100)   NOT NULL,
    [SendingStatusId]    INT              NULL,
    [SendingDateToBuyer] DATETIME         NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SampleInvoice] PRIMARY KEY CLUSTERED ([SampleInvoiceId] ASC),
    CONSTRAINT [FK_Mrc_SampleInvoice_Mrc_SendingStatus] FOREIGN KEY ([SendingStatusId]) REFERENCES [dbo].[Mrc_SendingStatus] ([SendingStatusId])
);

