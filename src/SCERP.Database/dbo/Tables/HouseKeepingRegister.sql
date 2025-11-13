CREATE TABLE [dbo].[HouseKeepingRegister] (
    [HouseKeepingRegisterId] INT              IDENTITY (1, 1) NOT NULL,
    [HouseKeepingItemId]     INT              NOT NULL,
    [Quantity]               FLOAT (53)       NULL,
    [Rate]                   FLOAT (53)       NULL,
    [EmployeeId]             UNIQUEIDENTIFIER NOT NULL,
    [IusseDate]              DATE             NULL,
    [Remarks]                VARCHAR (250)    NULL,
    [ReturnQty]              FLOAT (53)       NOT NULL,
    [ReturnDate]             DATE             NULL,
    [CompId]                 VARCHAR (3)      NOT NULL,
    CONSTRAINT [PK_HouseKeepingRegister] PRIMARY KEY CLUSTERED ([HouseKeepingRegisterId] ASC),
    CONSTRAINT [FK_HouseKeepingRegister_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]),
    CONSTRAINT [FK_HouseKeepingRegister_HouseKeepingItem1] FOREIGN KEY ([HouseKeepingItemId]) REFERENCES [dbo].[HouseKeepingItem] ([HouseKeepingItemId])
);

