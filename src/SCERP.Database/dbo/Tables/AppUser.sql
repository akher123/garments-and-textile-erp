CREATE TABLE [dbo].[AppUser] (
    [UserId]   INT           IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (50) NULL,
    [Password] NVARCHAR (50) NULL,
    CONSTRAINT [PK_AppUser] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

