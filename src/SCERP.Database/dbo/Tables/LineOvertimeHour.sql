CREATE TABLE [dbo].[LineOvertimeHour] (
    [LineOvertimeHourId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [CompanyId]          INT              NULL,
    [TransactionDate]    DATE             NOT NULL,
    [DepartmentLineId]   INT              NOT NULL,
    [Line]               VARCHAR (100)    NULL,
    [OvertimePerson]     INT              NOT NULL,
    [OvertimeHour]       NUMERIC (38, 2)  NOT NULL,
    [TTLOtAmount]        FLOAT (53)       NOT NULL,
    [FirstSign]          CHAR (1)         NOT NULL,
    [SecondSign]         CHAR (1)         NOT NULL,
    [FirstSignBy]        UNIQUEIDENTIFIER NULL,
    [SeconSignBy]        UNIQUEIDENTIFIER NULL,
    [PrepairedBy]        UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_LineOvertimeHour] PRIMARY KEY CLUSTERED ([LineOvertimeHourId] ASC)
);

