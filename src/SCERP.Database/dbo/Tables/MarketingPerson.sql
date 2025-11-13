CREATE TABLE [dbo].[MarketingPerson] (
    [MarketingPersonId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR (150) NULL,
    [Phone]             VARCHAR (50)  NULL,
    [Email]             VARCHAR (50)  NULL,
    [Designation]       VARCHAR (50)  NULL,
    [JoiningDate]       DATE          NULL,
    [QuitDate]          DATE          NULL,
    [IsActive]          BIT           NOT NULL,
    CONSTRAINT [PK_MarketingPerson] PRIMARY KEY CLUSTERED ([MarketingPersonId] ASC)
);

