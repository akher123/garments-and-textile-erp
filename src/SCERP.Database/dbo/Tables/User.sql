CREATE TABLE [dbo].[User] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [UserName]       NVARCHAR (100)   NOT NULL,
    [EmailAddress]   VARCHAR (100)    NULL,
    [EmployeeId]     UNIQUEIDENTIFIER NULL,
    [PasswordHash]   BINARY (64)      NOT NULL,
    [Salt]           UNIQUEIDENTIFIER NULL,
    [CDT]            DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EDT]            DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NULL,
    [TnaResponsible] VARCHAR (50)     NULL,
    [ContactId]      UNIQUEIDENTIFIER NOT NULL,
    [IsSystemUser]   BIT              NOT NULL,
    [DomainName]     VARCHAR (50)     NULL,
    [CompId]         CHAR (3)         NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserName] ASC),
    CONSTRAINT [FK_User_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

