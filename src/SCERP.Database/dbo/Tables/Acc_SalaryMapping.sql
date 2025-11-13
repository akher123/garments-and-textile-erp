CREATE TABLE [dbo].[Acc_SalaryMapping] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [SectorId]     INT              NULL,
    [CostCentreId] INT              NULL,
    [SalaryHeadId] INT              NULL,
    [GLID]         INT              NULL,
    [IsActive]     BIT              NULL,
    [CDT]          DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EDT]          DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Acc_SalaryMapping] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Acc_SalaryMapping_Acc_CompanySector] FOREIGN KEY ([SectorId]) REFERENCES [dbo].[Acc_CompanySector] ([Id]),
    CONSTRAINT [FK_Acc_SalaryMapping_Acc_CostCentre] FOREIGN KEY ([CostCentreId]) REFERENCES [dbo].[Acc_CostCentre] ([Id]),
    CONSTRAINT [FK_Acc_SalaryMapping_Acc_GLAccounts] FOREIGN KEY ([GLID]) REFERENCES [dbo].[Acc_GLAccounts_Back] ([Id]),
    CONSTRAINT [FK_Acc_SalaryMapping_SalaryHead] FOREIGN KEY ([SalaryHeadId]) REFERENCES [dbo].[SalaryHead] ([Id])
);

