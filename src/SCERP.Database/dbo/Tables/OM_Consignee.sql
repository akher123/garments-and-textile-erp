CREATE TABLE [dbo].[OM_Consignee] (
    [ConsigneeId]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompId]          VARCHAR (3)    NULL,
    [ConsigneeRefId]  VARCHAR (3)    NOT NULL,
    [ConsigneeName]   NVARCHAR (100) NOT NULL,
    [Address1]        NVARCHAR (100) NULL,
    [Address2]        NVARCHAR (100) NULL,
    [Address3]        NVARCHAR (100) NULL,
    [ConsigneeLookup] VARCHAR (10)   NULL,
    [PackList]        VARCHAR (500)  NULL,
    [Remarks]         VARCHAR (50)   NULL,
    [SourceID]        INT            NULL,
    [BuyerRefId]      VARCHAR (5)    NOT NULL,
    [CItyId]          INT            NOT NULL,
    [CountryId]       INT            NOT NULL,
    [ShippingMarks]   VARCHAR (50)   NULL,
    [Suppliercode]    VARCHAR (25)   NULL,
    [Phone]           VARCHAR (25)   NULL,
    [Fax]             VARCHAR (25)   NULL,
    [EMail]           VARCHAR (50)   NULL,
    CONSTRAINT [PK_OM_Consignee] PRIMARY KEY CLUSTERED ([ConsigneeId] ASC)
);

