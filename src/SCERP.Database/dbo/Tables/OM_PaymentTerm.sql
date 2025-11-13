CREATE TABLE [dbo].[OM_PaymentTerm] (
    [PayentTermId]  INT             IDENTITY (1, 1) NOT NULL,
    [CompId]        VARCHAR (3)     NULL,
    [PayTermRefId]  VARCHAR (2)     NOT NULL,
    [PayTerm]       NVARCHAR (100)  NULL,
    [ECGCPerc]      DECIMAL (18, 2) NOT NULL,
    [InsurancePerc] DECIMAL (18, 2) NULL,
    [CreditDays]    INT             NULL,
    [PayType]       VARCHAR (2)     NULL,
    CONSTRAINT [PK_OM_PaymentTerm] PRIMARY KEY CLUSTERED ([PayentTermId] ASC)
);

