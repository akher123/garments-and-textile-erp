CREATE TABLE [dbo].[OM_Style] (
    [StyleId]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompID]     VARCHAR (3)     NOT NULL,
    [StylerefId] VARCHAR (4)     NULL,
    [StyleName]  NVARCHAR (100)  NULL,
    [ItemId]     INT             NOT NULL,
    [Rate]       NUMERIC (12, 2) NULL,
    [SourceId]   INT             NULL,
    [Department] VARCHAR (40)    NULL,
    [Season]     VARCHAR (40)    NULL,
    [EnquiryId]  INT             NULL,
    [TypeId]     VARCHAR (2)     NULL,
    [CatId]      VARCHAR (2)     NULL,
    [PrdId]      VARCHAR (4)     NULL,
    CONSTRAINT [PK_OM_Style] PRIMARY KEY CLUSTERED ([StyleId] ASC)
);

