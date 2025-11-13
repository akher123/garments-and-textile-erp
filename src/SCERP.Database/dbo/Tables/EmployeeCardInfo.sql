CREATE TABLE [dbo].[EmployeeCardInfo] (
    [CardSerialId] INT            IDENTITY (1, 1) NOT NULL,
    [CompanyId]    INT            NOT NULL,
    [Address1]     NVARCHAR (200) NULL,
    [Address2]     NVARCHAR (200) NULL,
    [Telephone]    NVARCHAR (100) NULL,
    [Fax]          NVARCHAR (100) NULL,
    [Mobile]       NVARCHAR (100) NULL,
    [Email]        NVARCHAR (100) NULL,
    [Website]      NVARCHAR (100) NULL,
    [IsBangla]     BIT            NOT NULL,
    [IsActive]     BIT            NOT NULL,
    CONSTRAINT [PK_EmployeeCardInfo] PRIMARY KEY CLUSTERED ([CardSerialId] ASC),
    CONSTRAINT [FK_EmployeeCardInfo_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id])
);

