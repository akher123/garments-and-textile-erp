CREATE TABLE [dbo].[Mrc_SupplierCompany] (
    [SupplierCompanyId] INT              IDENTITY (1, 1) NOT NULL,
    [CompanyName]       NVARCHAR (100)   NOT NULL,
    [ContactName]       NVARCHAR (200)   NULL,
    [SupplierCode]      NVARCHAR (100)   NULL,
    [Address]           NVARCHAR (MAX)   NULL,
    [Email]             NVARCHAR (100)   NULL,
    [Phone]             NVARCHAR (100)   NULL,
    [Web]               NVARCHAR (200)   NULL,
    [Fax]               NVARCHAR (100)   NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SupplierCompany] PRIMARY KEY CLUSTERED ([SupplierCompanyId] ASC)
);

