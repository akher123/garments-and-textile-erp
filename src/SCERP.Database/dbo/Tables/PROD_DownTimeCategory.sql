CREATE TABLE [dbo].[PROD_DownTimeCategory] (
    [DownTimeCategoryId] INT           IDENTITY (1, 1) NOT NULL,
    [CategoryName]       VARCHAR (150) NOT NULL,
    [Code]               VARCHAR (20)  NOT NULL,
    [CompId]             VARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_PROD_DownTimeCategory] PRIMARY KEY CLUSTERED ([DownTimeCategoryId] ASC)
);

