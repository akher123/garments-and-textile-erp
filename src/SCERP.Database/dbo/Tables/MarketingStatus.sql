CREATE TABLE [dbo].[MarketingStatus] (
    [StatusId]   INT           IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_MarketingStatus] PRIMARY KEY CLUSTERED ([StatusId] ASC)
);

