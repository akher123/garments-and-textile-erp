CREATE TABLE [dbo].[EmployeeSalarySummary] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [SLNO]           NVARCHAR (2)   NULL,
    [EmployeeTypeId] NVARCHAR (2)   NULL,
    [EmployeeType]   NVARCHAR (100) NULL,
    [DepartmentId]   NVARCHAR (2)   NULL,
    [Department]     NVARCHAR (100) NULL,
    [SectionId]      NVARCHAR (2)   NULL,
    [Section]        NVARCHAR (100) NULL,
    [LineId]         INT            NULL,
    [Line]           NVARCHAR (100) NULL,
    [NetAmount]      FLOAT (53)     NULL,
    [SumEmployee]    INT            NULL,
    [ExtraOT]        FLOAT (53)     NULL,
    [RegularOT]      FLOAT (53)     NULL,
    [ExtraOTHours]   FLOAT (53)     NULL,
    [RegularOTHours] FLOAT (53)     NULL,
    CONSTRAINT [PK_EmployeeSalarySummary] PRIMARY KEY CLUSTERED ([Id] ASC)
);

