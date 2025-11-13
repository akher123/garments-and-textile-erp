CREATE TABLE [dbo].[AuthorizedPerson] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]          UNIQUEIDENTIFIER NOT NULL,
    [AuthorizationTypeId] INT              NOT NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL,
    CONSTRAINT [PK_AuthorizedPerson] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AuthorizedPerson_AuthorizationType] FOREIGN KEY ([AuthorizationTypeId]) REFERENCES [dbo].[AuthorizationType] ([Id]),
    CONSTRAINT [FK_AuthorizedPerson_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

