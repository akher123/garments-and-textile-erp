CREATE TABLE [dbo].[Acc_ControlAccounts] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [ParentCode]   NUMERIC (18)   NOT NULL,
    [ControlCode]  NUMERIC (18)   NOT NULL,
    [ControlName]  NVARCHAR (500) NOT NULL,
    [ControlLevel] INT            NULL,
    [SortOrder]    INT            NULL,
    [IsActive]     BIT            NULL,
    CONSTRAINT [PK_Acc_ControlAccounts] PRIMARY KEY CLUSTERED ([Id] ASC)
);

