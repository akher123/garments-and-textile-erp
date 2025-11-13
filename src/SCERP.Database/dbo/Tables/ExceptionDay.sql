CREATE TABLE [dbo].[ExceptionDay] (
    [ExceptionDayId]           INT              IDENTITY (1, 1) NOT NULL,
    [BranchUnitId]             INT              NOT NULL,
    [ExceptionDate]            DATETIME         NOT NULL,
    [IsExceptionForWeekend]    BIT              NULL,
    [IsExceptionForHoliday]    BIT              NULL,
    [IsExceptionForGeneralDay] BIT              NULL,
    [IsDeclaredAsWeekend]      BIT              NULL,
    [IsDeclaredAsHoliday]      BIT              NULL,
    [IsDeclaredAsGeneralDay]   BIT              NULL,
    [Remarks]                  NVARCHAR (MAX)   NULL,
    [CreatedDate]              DATETIME         NULL,
    [CreatedBy]                UNIQUEIDENTIFIER NULL,
    [EditedDate]               DATETIME         NULL,
    [EditedBy]                 UNIQUEIDENTIFIER NULL,
    [IsActive]                 BIT              NOT NULL,
    CONSTRAINT [PK_ExceptionDay] PRIMARY KEY CLUSTERED ([ExceptionDayId] ASC),
    CONSTRAINT [FK_ExceptionDay_BranchUnit] FOREIGN KEY ([BranchUnitId]) REFERENCES [dbo].[BranchUnit] ([BranchUnitId])
);

