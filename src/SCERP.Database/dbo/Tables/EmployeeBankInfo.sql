CREATE TABLE [dbo].[EmployeeBankInfo] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]    UNIQUEIDENTIFIER NOT NULL,
    [BankName]      NVARCHAR (100)   NOT NULL,
    [BranchName]    NVARCHAR (100)   NULL,
    [AccountTypeId] INT              NULL,
    [AccountName]   NVARCHAR (100)   NOT NULL,
    [AccountNumber] NVARCHAR (100)   NOT NULL,
    [FromDate]      DATETIME         NOT NULL,
    [ToDate]        DATETIME         NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeBankInfo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

