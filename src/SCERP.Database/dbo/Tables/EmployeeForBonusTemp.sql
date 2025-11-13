CREATE TABLE [dbo].[EmployeeForBonusTemp] (
    [SerialNo]       INT             IDENTITY (1, 1) NOT NULL,
    [EmployeeCardId] NVARCHAR (50)   NULL,
    [Name]           NVARCHAR (50)   NULL,
    [Designation]    NVARCHAR (50)   NULL,
    [Grade]          NVARCHAR (50)   NULL,
    [Department]     NVARCHAR (50)   NULL,
    [Section]        NVARCHAR (50)   NULL,
    [Line]           NVARCHAR (50)   NULL,
    [JoiningDate]    VARCHAR (10)    NULL,
    [BonusDate]      VARCHAR (10)    NULL,
    [GrossSalary]    NUMERIC (18, 2) NULL,
    [BasicSalary]    NUMERIC (18, 2) NULL,
    [ServiceLength]  INT             NULL,
    [Percentage]     NUMERIC (18, 2) NULL,
    [BonusAmount]    NUMERIC (18, 2) NULL,
    [CreatedDate]    DATETIME        NULL,
    CONSTRAINT [PK_EmployeeBonusTemp] PRIMARY KEY CLUSTERED ([SerialNo] ASC)
);

