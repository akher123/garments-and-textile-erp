CREATE TABLE [dbo].[MarketingInquiry] (
    [InquiryId]            INT            IDENTITY (1, 1) NOT NULL,
    [InquiryDate]          DATETIME       NULL,
    [InstituteId]          INT            NOT NULL,
    [InquiryContactPerson] NVARCHAR (150) NULL,
    [Mobile]               NVARCHAR (100) NULL,
    [Telephone]            NVARCHAR (100) NULL,
    [Email]                NVARCHAR (200) NULL,
    [FurtherContactType]   VARCHAR (50)   NULL,
    [Remarks]              NVARCHAR (MAX) NULL,
    [IsActive]             BIT            NOT NULL,
    [Amount]               FLOAT (53)     NULL,
    [OthersAmount]         FLOAT (53)     NULL,
    [BillNo]               VARCHAR (50)   NULL,
    [MarketingPersonId]    INT            NOT NULL,
    CONSTRAINT [PK_MarketingInquiry] PRIMARY KEY CLUSTERED ([InquiryId] ASC),
    CONSTRAINT [FK_MarketingInquiry_MarketingInstitute] FOREIGN KEY ([InstituteId]) REFERENCES [dbo].[MarketingInstitute] ([InstituteId]),
    CONSTRAINT [FK_MarketingInquiry_MarketingPerson] FOREIGN KEY ([MarketingPersonId]) REFERENCES [dbo].[MarketingPerson] ([MarketingPersonId])
);

