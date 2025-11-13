CREATE TABLE [dbo].[PreviousEarnLeave] (
    [Id]                INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeCardId]    NVARCHAR (50)    NULL,
    [EmployeeId]        UNIQUEIDENTIFIER NULL,
    [PreviousEarnLeave] DECIMAL (18, 2)  NULL,
    [IsActive]          BIT              NULL,
    CONSTRAINT [PK_PreviousEarnLeave] PRIMARY KEY CLUSTERED ([Id] ASC)
);

