CREATE TABLE [dbo].[OM_Category] (
    [CatergoryId] INT            IDENTITY (1, 1) NOT NULL,
    [CompId]      NVARCHAR (3)   NULL,
    [CatRefId]    NVARCHAR (3)   NULL,
    [CatName]     NVARCHAR (50)  NULL,
    [Quota]       BIT            NULL,
    [SourceId]    INT            NULL,
    [QuotaUnit]   INT            NULL,
    [EqualPcs]    NUMERIC (4, 3) NULL,
    CONSTRAINT [PK_OM_Category] PRIMARY KEY CLUSTERED ([CatergoryId] ASC)
);

