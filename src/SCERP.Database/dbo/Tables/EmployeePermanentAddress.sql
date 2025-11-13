CREATE TABLE [dbo].[EmployeePermanentAddress] (
    [Id]                      INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]              UNIQUEIDENTIFIER NOT NULL,
    [MailingAddress]          NVARCHAR (MAX)   NOT NULL,
    [MailingAddressInBengali] NVARCHAR (MAX)   NOT NULL,
    [CountryId]               INT              NULL,
    [DistrictId]              INT              NULL,
    [PoliceStationId]         INT              NULL,
    [PostOffice]              NVARCHAR (100)   NULL,
    [PostOfficeInBengali]     NVARCHAR (100)   NULL,
    [PostCode]                NVARCHAR (100)   NULL,
    [CreatedDate]             DATETIME         NULL,
    [CreatedBy]               UNIQUEIDENTIFIER NULL,
    [EditedDate]              DATETIME         NULL,
    [EditedBy]                UNIQUEIDENTIFIER NULL,
    [Status]                  INT              NOT NULL,
    [IsActive]                BIT              NOT NULL,
    CONSTRAINT [PK_EmployeePermanentAddress] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeePermanentAddress_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeePermanentAddress_District] FOREIGN KEY ([DistrictId]) REFERENCES [dbo].[District] ([Id]),
    CONSTRAINT [FK_EmployeePermanentAddress_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeePermanentAddress_PoliceStation] FOREIGN KEY ([PoliceStationId]) REFERENCES [dbo].[PoliceStation] ([Id])
);

