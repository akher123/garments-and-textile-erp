CREATE TABLE [dbo].[WorkGroupRoster] (
    [WorkGroupRosterId] INT              IDENTITY (1, 1) NOT NULL,
    [UnitName]          NVARCHAR (50)    NULL,
    [GroupName]         NVARCHAR (50)    NULL,
    [EmployeeId]        UNIQUEIDENTIFIER NULL,
    [EmployeeCardId]    NVARCHAR (50)    NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_WorkGroupRoster] PRIMARY KEY CLUSTERED ([WorkGroupRosterId] ASC)
);

