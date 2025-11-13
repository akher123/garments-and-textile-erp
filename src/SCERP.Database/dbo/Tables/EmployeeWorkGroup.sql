CREATE TABLE [dbo].[EmployeeWorkGroup] (
    [EmployeeWorkGroupId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]          UNIQUEIDENTIFIER NOT NULL,
    [WorkGroupId]         INT              NOT NULL,
    [AssignedDate]        DATETIME         NOT NULL,
    [Status]              BIT              NOT NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeWorkGroup] PRIMARY KEY CLUSTERED ([EmployeeWorkGroupId] ASC),
    CONSTRAINT [FK_EmployeeWorkGroup_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]),
    CONSTRAINT [FK_EmployeeWorkGroup_WorkGroup] FOREIGN KEY ([WorkGroupId]) REFERENCES [dbo].[WorkGroup] ([WorkGroupId])
);

