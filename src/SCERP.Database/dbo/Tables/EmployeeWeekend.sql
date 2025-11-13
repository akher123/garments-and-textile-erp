CREATE TABLE [dbo].[EmployeeWeekend] (
    [EmployeeWeekendId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]        UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId]    NVARCHAR (50)    NOT NULL,
    [FromDate]          DATETIME         NOT NULL,
    [ToDate]            DATETIME         NOT NULL,
    [Comments]          NVARCHAR (MAX)   NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeWeekend] PRIMARY KEY CLUSTERED ([EmployeeWeekendId] ASC)
);

