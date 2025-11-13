CREATE TABLE [dbo].[CompanyOrganogram] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [DesignationId]       INT              NOT NULL,
    [ParentDesignationId] INT              NULL,
    [CDT]                 DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EDT]                 DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NULL,
    CONSTRAINT [PK_CompanyOrganogram] PRIMARY KEY CLUSTERED ([DesignationId] ASC),
    CONSTRAINT [FK_CompanyOrganogram_EmployeeDesignation] FOREIGN KEY ([DesignationId]) REFERENCES [dbo].[EmployeeDesignation] ([Id])
);

