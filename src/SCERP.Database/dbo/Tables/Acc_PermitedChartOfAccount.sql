CREATE TABLE [dbo].[Acc_PermitedChartOfAccount] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [SectorId]     INT          NOT NULL,
    [ControlCode]  NUMERIC (18) NOT NULL,
    [ControlLevel] INT          NOT NULL,
    [IsActive]     BIT          NOT NULL,
    CONSTRAINT [PK_Acc_PermitedChartOfAccount] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Acc_PermitedChartOfAccount_Acc_CompanySector] FOREIGN KEY ([SectorId]) REFERENCES [dbo].[Acc_CompanySector] ([Id])
);

