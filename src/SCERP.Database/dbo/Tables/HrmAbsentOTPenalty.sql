CREATE TABLE [dbo].[HrmAbsentOTPenalty] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]     UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId] NVARCHAR (10)    NOT NULL,
    [EmployeeName]   NVARCHAR (100)   NOT NULL,
    [Designation]    NVARCHAR (100)   NOT NULL,
    [Department]     NVARCHAR (100)   NOT NULL,
    [Section]        NVARCHAR (100)   NULL,
    [Line]           NVARCHAR (100)   NULL,
    [EmployeeType]   NVARCHAR (50)    NULL,
    [JoinDate]       DATE             NOT NULL,
    [Date]           DATE             NULL,
    [Amount]         DECIMAL (18, 5)  NULL,
    [OTDeduction]    DECIMAL (18, 5)  NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_HrmAbsentOTPenalty] PRIMARY KEY CLUSTERED ([Id] ASC)
);

