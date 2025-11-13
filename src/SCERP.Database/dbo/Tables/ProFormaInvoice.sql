CREATE TABLE [dbo].[ProFormaInvoice] (
    [PiId]         INT              IDENTITY (1, 1) NOT NULL,
    [CompId]       CHAR (3)         NOT NULL,
    [PiRefId]      VARCHAR (7)      NOT NULL,
    [PiNo]         VARCHAR (100)    NOT NULL,
    [SupplierId]   INT              NOT NULL,
    [ReceivedDate] DATE             NULL,
    [BookingNo]    VARCHAR (50)     NULL,
    [EndDate]      DATE             NULL,
    [PType]        CHAR (1)         NOT NULL,
    [Remarks]      VARCHAR (250)    NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [CreatedDate]  DATE             NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATE             NULL,
    CONSTRAINT [PK_ProFormaInvoice] PRIMARY KEY CLUSTERED ([PiId] ASC),
    CONSTRAINT [FK_ProFormaInvoice_Mrc_SupplierCompany] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Mrc_SupplierCompany] ([SupplierCompanyId])
);

