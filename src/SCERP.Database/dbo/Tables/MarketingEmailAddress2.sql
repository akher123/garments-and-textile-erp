CREATE TABLE [dbo].[MarketingEmailAddress2] (
    [SerialId]       INT            IDENTITY (1, 1) NOT NULL,
    [Factory Name]   NVARCHAR (255) NULL,
    [Contact Person] NVARCHAR (255) NULL,
    [Address]        NVARCHAR (255) NULL,
    [Email Address]  NVARCHAR (255) NULL,
    [Contact Number] NVARCHAR (255) NULL,
    CONSTRAINT [PK_MarketingEmailAddress2] PRIMARY KEY CLUSTERED ([SerialId] ASC)
);

