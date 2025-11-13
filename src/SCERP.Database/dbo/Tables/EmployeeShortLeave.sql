CREATE TABLE [dbo].[EmployeeShortLeave] (
    [Id]                INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]        UNIQUEIDENTIFIER NOT NULL,
    [ReasonType]        TINYINT          NOT NULL,
    [ReasonDescription] NVARCHAR (MAX)   NOT NULL,
    [Date]              DATETIME         NOT NULL,
    [FromTime]          TIME (7)         NOT NULL,
    [ToTime]            TIME (7)         NOT NULL,
    [TotalHours]        TIME (7)         NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeShortLeave_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeShortLeave_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

