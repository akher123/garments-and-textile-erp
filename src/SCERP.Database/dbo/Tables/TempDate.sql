CREATE TABLE [dbo].[TempDate] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [MonthDate]  DATETIME         NULL,
    [MonthDay]   NVARCHAR (100)   NULL,
    [EmployeeId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_TempDate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

