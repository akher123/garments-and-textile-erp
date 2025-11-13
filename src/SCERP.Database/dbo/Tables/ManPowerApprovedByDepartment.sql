CREATE TABLE [dbo].[ManPowerApprovedByDepartment] (
    [ManPowerApprovedByDepartmentId] INT              IDENTITY (1, 1) NOT NULL,
    [DepartmentId]                   INT              NULL,
    [DepartmentName]                 NVARCHAR (100)   NULL,
    [ApprovedManPower]               INT              NULL,
    [CreatedDate]                    DATETIME         NULL,
    [CreatedBy]                      UNIQUEIDENTIFIER NULL,
    [EditedDate]                     DATETIME         NULL,
    [EditedBy]                       UNIQUEIDENTIFIER NULL,
    [IsActive]                       BIT              NULL,
    CONSTRAINT [PK_ManPowerApprovedByDepartment] PRIMARY KEY CLUSTERED ([ManPowerApprovedByDepartmentId] ASC)
);

