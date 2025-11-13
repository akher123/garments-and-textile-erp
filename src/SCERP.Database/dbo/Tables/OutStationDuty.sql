CREATE TABLE [dbo].[OutStationDuty] (
    [OutStationDutyId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]       UNIQUEIDENTIFIER NOT NULL,
    [DutyDate]         DATETIME         NOT NULL,
    [Location]         NVARCHAR (100)   NOT NULL,
    [Purpose]          NVARCHAR (MAX)   NOT NULL,
    [CreatedDate]      DATETIME         NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]       DATETIME         NULL,
    [EditedBy]         UNIQUEIDENTIFIER NULL,
    [IsActive]         BIT              NOT NULL,
    CONSTRAINT [PK_OutStationDuty] PRIMARY KEY CLUSTERED ([OutStationDutyId] ASC),
    CONSTRAINT [FK_OutStationDuty_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

