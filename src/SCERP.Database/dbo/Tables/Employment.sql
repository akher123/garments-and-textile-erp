CREATE TABLE [dbo].[Employment] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]       UNIQUEIDENTIFIER NOT NULL,
    [CompanyName]      NVARCHAR (100)   NOT NULL,
    [CompanyAddress]   NVARCHAR (100)   NULL,
    [Department]       NVARCHAR (100)   NULL,
    [Designation]      NVARCHAR (100)   NOT NULL,
    [Respinsibilities] NVARCHAR (MAX)   NULL,
    [FromDate]         DATETIME         NULL,
    [ToDate]           DATETIME         NULL,
    [CreatedDate]      DATETIME         NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]       DATETIME         NULL,
    [EditedBy]         UNIQUEIDENTIFIER NULL,
    [IsActive]         BIT              NOT NULL,
    CONSTRAINT [PK_Employment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employment_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

