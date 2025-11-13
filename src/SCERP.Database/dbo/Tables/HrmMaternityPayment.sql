CREATE TABLE [dbo].[HrmMaternityPayment] (
    [MaternityPaymentId]  INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]          UNIQUEIDENTIFIER NOT NULL,
    [LeaveDayStart]       DATETIME         NULL,
    [LeaveDayEnd]         DATETIME         NULL,
    [FirstPaymentDate]    DATETIME         NULL,
    [FirstPaymentAmount]  DECIMAL (18, 2)  NULL,
    [SecondPaymentDate]   DATETIME         NULL,
    [SecondPaymentAmount] DECIMAL (18, 2)  NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL,
    [CompId]              VARCHAR (3)      NOT NULL,
    CONSTRAINT [PK_HrmMaternityPayment] PRIMARY KEY CLUSTERED ([MaternityPaymentId] ASC)
);

