CREATE TABLE [dbo].[SalaryHead] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [SalaryHeadName]        NVARCHAR (100) NOT NULL,
    [SalaryHeadDescription] NVARCHAR (100) NULL,
    [IsActive]              BIT            NULL,
    CONSTRAINT [PK_SalaryHead] PRIMARY KEY CLUSTERED ([Id] ASC)
);

