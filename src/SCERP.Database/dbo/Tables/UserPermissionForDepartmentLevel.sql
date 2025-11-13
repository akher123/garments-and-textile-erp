CREATE TABLE [dbo].[UserPermissionForDepartmentLevel] (
    [UserPermissionForDepartmentLevelId] INT              IDENTITY (1, 1) NOT NULL,
    [UserName]                           NVARCHAR (100)   NOT NULL,
    [CompanyId]                          INT              NOT NULL,
    [BranchId]                           INT              NOT NULL,
    [BranchUnitId]                       INT              NOT NULL,
    [BranchUnitDepartmentId]             INT              NOT NULL,
    [CreatedDate]                        DATETIME         NULL,
    [CreatedBy]                          UNIQUEIDENTIFIER NULL,
    [EditedDate]                         DATETIME         NULL,
    [EditedBy]                           UNIQUEIDENTIFIER NULL,
    [IsActive]                           BIT              NOT NULL,
    CONSTRAINT [PK_UserPermission] PRIMARY KEY CLUSTERED ([UserPermissionForDepartmentLevelId] ASC),
    CONSTRAINT [FK_UserPermissionForDepartmentLevel_Branch] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch] ([Id]),
    CONSTRAINT [FK_UserPermissionForDepartmentLevel_BranchUnit] FOREIGN KEY ([BranchUnitId]) REFERENCES [dbo].[BranchUnit] ([BranchUnitId]),
    CONSTRAINT [FK_UserPermissionForDepartmentLevel_BranchUnitDepartment] FOREIGN KEY ([BranchUnitDepartmentId]) REFERENCES [dbo].[BranchUnitDepartment] ([BranchUnitDepartmentId]),
    CONSTRAINT [FK_UserPermissionForDepartmentLevel_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserPermissionForDepartmentLevel_User] FOREIGN KEY ([UserName]) REFERENCES [dbo].[User] ([UserName])
);

