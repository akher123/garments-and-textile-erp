CREATE TABLE [dbo].[CommBbLcItemDetails] (
    [BbLcItemDetailsId] INT            IDENTITY (1, 1) NOT NULL,
    [BbLcId]            INT            NOT NULL,
    [Item]              NVARCHAR (100) NULL,
    [Specification]     NVARCHAR (100) NULL,
    [Quantity]          FLOAT (53)     NULL,
    [Rate]              FLOAT (53)     NULL,
    [Remarks]           NVARCHAR (100) NULL,
    CONSTRAINT [PK_CommBbLcItemDetails] PRIMARY KEY CLUSTERED ([BbLcItemDetailsId] ASC),
    CONSTRAINT [FK_CommBbLcItemDetails_CommBbLcInfo] FOREIGN KEY ([BbLcId]) REFERENCES [dbo].[CommBbLcInfo] ([BbLcId])
);

