CREATE TABLE [dbo].[Acc_DepreciationChart] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [ControlCode]      NUMERIC (18)     NULL,
    [ControlName]      NVARCHAR (200)   NULL,
    [DepreciationRate] NUMERIC (18, 2)  NULL,
    [CDT]              DATETIME         NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [EDT]              DATETIME         NULL,
    [EditedBy]         UNIQUEIDENTIFIER NULL,
    [IsActive]         BIT              NULL,
    CONSTRAINT [PK_Acc_DepreciationChart] PRIMARY KEY CLUSTERED ([Id] ASC)
);

