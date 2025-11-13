CREATE TABLE [dbo].[OM_Agent] (
    [AgentId]    INT            IDENTITY (1, 1) NOT NULL,
    [CompId]     VARCHAR (3)    NULL,
    [AgentRefId] VARCHAR (3)    NOT NULL,
    [AgentName]  NVARCHAR (100) NULL,
    [AType]      VARCHAR (5)    NULL,
    [Address1]   NVARCHAR (100) NULL,
    [Address2]   NVARCHAR (100) NULL,
    [Address3]   NVARCHAR (100) NULL,
    [CityId]     INT            NOT NULL,
    [CountryId]  INT            NOT NULL,
    [Phone]      VARCHAR (25)   NULL,
    [Fax]        VARCHAR (25)   NULL,
    [EMail]      VARCHAR (50)   NULL,
    CONSTRAINT [PK_OM_Agent] PRIMARY KEY CLUSTERED ([AgentId] ASC)
);

