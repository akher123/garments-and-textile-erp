CREATE TABLE [dbo].[Inventory_AuthorizedPerson] (
    [AuthorizedPersonId] INT              IDENTITY (1, 1) NOT NULL,
    [ProcessName]        NVARCHAR (100)   NOT NULL,
    [ProcessTypeName]    NVARCHAR (100)   NOT NULL,
    [ProcessId]          INT              NOT NULL,
    [ProcessTypeId]      INT              NOT NULL,
    [EmployeeCardId]     NVARCHAR (50)    NULL,
    [EmployeeId]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedDate]         DATETIME         NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              CONSTRAINT [DF_Inventory_AuthorizedPerson_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Inventory_AuthorizedPerson] PRIMARY KEY CLUSTERED ([AuthorizedPersonId] ASC),
    CONSTRAINT [FK_Inventory_AuthorizedPerson_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

