CREATE TABLE [dbo].[UserPermissionForEmployeeLevel] (
    [UserPermissionForEmployeeLevelId] INT              IDENTITY (1, 1) NOT NULL,
    [UserName]                         NVARCHAR (100)   NOT NULL,
    [EmployeeTypeId]                   INT              NOT NULL,
    [CreatedDate]                      DATETIME         NULL,
    [CreatedBy]                        UNIQUEIDENTIFIER NULL,
    [EditedDate]                       DATETIME         NULL,
    [EditedBy]                         UNIQUEIDENTIFIER NULL,
    [IsActive]                         BIT              NOT NULL,
    CONSTRAINT [PK_UserPermissionForEmployeeLevel] PRIMARY KEY CLUSTERED ([UserPermissionForEmployeeLevelId] ASC),
    CONSTRAINT [FK_UserPermissionForEmployeeLevel_EmployeeType] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [dbo].[EmployeeType] ([Id]),
    CONSTRAINT [FK_UserPermissionForEmployeeLevel_User] FOREIGN KEY ([UserName]) REFERENCES [dbo].[User] ([UserName])
);

