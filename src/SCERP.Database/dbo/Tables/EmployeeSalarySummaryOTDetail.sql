CREATE TABLE [dbo].[EmployeeSalarySummaryOTDetail] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [SLNO]            NVARCHAR (2)   NULL,
    [EmployeeTypeId]  NVARCHAR (2)   NULL,
    [EmployeeType]    NVARCHAR (100) NULL,
    [DepartmentId]    NVARCHAR (2)   NULL,
    [Department]      NVARCHAR (100) NULL,
    [SectionId]       NVARCHAR (2)   NULL,
    [Section]         NVARCHAR (100) NULL,
    [LineId]          INT            NULL,
    [Line]            NVARCHAR (100) NULL,
    [NetAmount]       FLOAT (53)     NULL,
    [SumEmployee]     INT            NULL,
    [RegularOTHour]   FLOAT (53)     NULL,
    [RegularOTAmount] FLOAT (53)     NULL,
    [ExtraOTHour]     FLOAT (53)     NULL,
    [ExtraOTAmount]   FLOAT (53)     NULL,
    [WeekendOTHour]   FLOAT (53)     NULL,
    [WeekendOTAmount] FLOAT (53)     NULL,
    [HolidayOTHour]   FLOAT (53)     NULL,
    [HolidayOTAmount] FLOAT (53)     NULL,
    CONSTRAINT [PK_EmployeeSalarySummaryOTDetail] PRIMARY KEY CLUSTERED ([Id] ASC)
);

