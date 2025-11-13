CREATE TABLE [dbo].[ManPowerApproved] (
    [Id]                   INT              IDENTITY (1, 1) NOT NULL,
    [DepartmentId]         INT              NULL,
    [Department]           NVARCHAR (100)   NULL,
    [DepartmentInBengali]  NVARCHAR (100)   NULL,
    [SectionId]            INT              NULL,
    [Section]              NVARCHAR (100)   NULL,
    [SectionInBengali]     NVARCHAR (100)   NULL,
    [EmployeeTypeId]       INT              NULL,
    [DesignationId]        INT              NULL,
    [Designation]          NVARCHAR (100)   NULL,
    [DesignationInBengali] NVARCHAR (100)   NULL,
    [ApprovedEmployee]     INT              NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NULL,
    CONSTRAINT [PK_ManPowerApproved] PRIMARY KEY CLUSTERED ([Id] ASC)
);

