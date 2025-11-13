CREATE TABLE [dbo].[EmployeeWorkGroupTemp] (
    [EmployeeWorkGroupId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]          UNIQUEIDENTIFIER NOT NULL,
    [WorkGroupId]         INT              NOT NULL,
    [AssignedDate]        DATETIME         NOT NULL,
    [Status]              BIT              NOT NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL
);

