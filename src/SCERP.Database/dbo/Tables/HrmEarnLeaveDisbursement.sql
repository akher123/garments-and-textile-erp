CREATE TABLE [dbo].[HrmEarnLeaveDisbursement] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]     UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId] NVARCHAR (10)    NOT NULL,
    [Days]           INT              NOT NULL,
    [Amount]         DECIMAL (18, 2)  NOT NULL,
    [Date]           DATE             NOT NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_HrmEarnLeaveDisbursement] PRIMARY KEY CLUSTERED ([Id] ASC)
);

