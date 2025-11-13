CREATE TABLE [dbo].[TiffinBill] (
    [TiffinBillId]   INT              IDENTITY (1, 1) NOT NULL,
    [Date]           DATETIME         NULL,
    [EmployeeId]     UNIQUEIDENTIFIER NULL,
    [EmployeeCardId] NVARCHAR (100)   NULL,
    [EmployeeName]   NVARCHAR (100)   NULL,
    [BranchName]     NVARCHAR (100)   NULL,
    [UnitName]       NVARCHAR (100)   NULL,
    [DepartmentName] NVARCHAR (100)   NULL,
    [LineName]       NVARCHAR (100)   NULL,
    [SectionName]    NVARCHAR (100)   NULL,
    [EmployeeType]   NVARCHAR (100)   NULL,
    [Designation]    NVARCHAR (100)   NULL,
    [InTime]         TIME (7)         NULL,
    [OutTime]        TIME (7)         NULL,
    [Amount]         DECIMAL (18, 2)  NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_TiffinBill] PRIMARY KEY CLUSTERED ([TiffinBillId] ASC)
);

