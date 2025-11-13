CREATE TABLE [dbo].[MobileUser] (
    [id]           INT            IDENTITY (1, 1) NOT NULL,
    [UserName]     NVARCHAR (100) NOT NULL,
    [EmailAddress] VARCHAR (100)  NOT NULL,
    [Password]     VARCHAR (100)  NOT NULL,
    CONSTRAINT [PK_MobileUser] PRIMARY KEY CLUSTERED ([id] ASC)
);

