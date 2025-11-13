CREATE TABLE [dbo].[EmployeeFamilyInfo] (
    [EmployeeFamilyInfoId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]           UNIQUEIDENTIFIER NOT NULL,
    [NameOfChild]          NVARCHAR (100)   NOT NULL,
    [NameOfChildInBengali] NVARCHAR (100)   NOT NULL,
    [DateOfBirth]          DATETIME         NOT NULL,
    [GenderId]             TINYINT          NOT NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeFamilyInfo] PRIMARY KEY CLUSTERED ([EmployeeFamilyInfoId] ASC)
);

