CREATE TABLE [dbo].[AttendanceBonus] (
    [AttendanceBonusId] INT              IDENTITY (1, 1) NOT NULL,
    [DesignationId]     INT              NOT NULL,
    [Amount]            DECIMAL (18, 2)  NOT NULL,
    [FromDate]          DATETIME         NULL,
    [ToDate]            DATETIME         NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_AttendanceBonus] PRIMARY KEY CLUSTERED ([AttendanceBonusId] ASC),
    CONSTRAINT [FK_AttendanceBonus_EmployeeDesignation] FOREIGN KEY ([DesignationId]) REFERENCES [dbo].[EmployeeDesignation] ([Id])
);

