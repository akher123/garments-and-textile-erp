CREATE TABLE [dbo].[OM_Buyer] (
    [BuyerId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [BuyerRefId] VARCHAR (3)    NOT NULL,
    [CompId]     VARCHAR (3)    NULL,
    [BuyerName]  NVARCHAR (100) NOT NULL,
    [Address1]   NVARCHAR (100) NULL,
    [Address2]   NVARCHAR (100) NULL,
    [Address3]   NVARCHAR (100) NULL,
    [CityId]     INT            NOT NULL,
    [CountryId]  INT            NOT NULL,
    [Phone]      VARCHAR (50)   NULL,
    [Fax]        VARCHAR (50)   NULL,
    [EMail]      VARCHAR (50)   NULL,
    [Remarks]    VARCHAR (100)  NULL,
    [xStatus]    VARCHAR (5)    NULL,
    [EmpId]      VARCHAR (5)    NULL,
    [ACode]      VARCHAR (5)    NULL,
    CONSTRAINT [PK_OM_Buyer] PRIMARY KEY CLUSTERED ([BuyerId] ASC)
);

