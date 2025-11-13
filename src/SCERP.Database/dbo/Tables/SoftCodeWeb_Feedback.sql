CREATE TABLE [dbo].[SoftCodeWeb_Feedback] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (100) NULL,
    [CompanyName]     NVARCHAR (100) NULL,
    [EmailAddress]    NVARCHAR (100) NULL,
    [PhoneNo]         NVARCHAR (100) NULL,
    [Message]         NVARCHAR (500) NULL,
    [CreatedDateTime] DATETIME       NULL,
    [IsActive]        BIT            NULL,
    CONSTRAINT [PK_SoftCodeWeb_Feedback] PRIMARY KEY CLUSTERED ([Id] ASC)
);

