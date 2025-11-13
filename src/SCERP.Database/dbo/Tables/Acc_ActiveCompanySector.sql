CREATE TABLE [dbo].[Acc_ActiveCompanySector] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]  UNIQUEIDENTIFIER NULL,
    [CompanyId]   INT              NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              NOT NULL,
    CONSTRAINT [PK_Acc_ActiveCompanySector] PRIMARY KEY CLUSTERED ([Id] ASC)
);

