CREATE TABLE [dbo].[Mrc_PaymentTerms] (
    [PaymentTermId]          INT              IDENTITY (1, 1) NOT NULL,
    [PaymentTermName]        NVARCHAR (100)   NOT NULL,
    [PaymentTermDescription] NVARCHAR (MAX)   NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_PaymentTerms] PRIMARY KEY CLUSTERED ([PaymentTermId] ASC)
);

