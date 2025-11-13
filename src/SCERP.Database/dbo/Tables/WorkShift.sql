CREATE TABLE [dbo].[WorkShift] (
    [WorkShiftId]          INT              IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (100)   NOT NULL,
    [NameDetail]           NVARCHAR (100)   NULL,
    [NameInBengali]        NVARCHAR (100)   NULL,
    [NameDetailInBengali]  NVARCHAR (100)   NULL,
    [StartTime]            TIME (7)         NOT NULL,
    [EndTime]              TIME (7)         NOT NULL,
    [InBufferTime]         INT              NOT NULL,
    [MaxBeforeTime]        INT              NOT NULL,
    [MaxAfterTime]         INT              NOT NULL,
    [ExceededMaxAfterTime] INT              NULL,
    [Description]          NVARCHAR (MAX)   NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_WorkShift] PRIMARY KEY CLUSTERED ([WorkShiftId] ASC)
);

