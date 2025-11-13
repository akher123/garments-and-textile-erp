CREATE TABLE [dbo].[EmployeeEntitlement] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]    UNIQUEIDENTIFIER NOT NULL,
    [EntitlementId] INT              NOT NULL,
    [FromDate]      DATETIME         NOT NULL,
    [ToDate]        DATETIME         NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeEntitlement] PRIMARY KEY CLUSTERED ([Id] ASC)
);

