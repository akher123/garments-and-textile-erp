CREATE TABLE [dbo].[Acc_CostCentreMultiLayer] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [ParentId]  INT            NOT NULL,
    [ItemId]    INT            NOT NULL,
    [ItemName]  NVARCHAR (100) NULL,
    [ItemLevel] INT            NULL,
    [SortOrder] INT            NULL,
    [IsActive]  BIT            NOT NULL,
    CONSTRAINT [PK_Acc_CostCentreMultiLayer] PRIMARY KEY CLUSTERED ([Id] ASC)
);

