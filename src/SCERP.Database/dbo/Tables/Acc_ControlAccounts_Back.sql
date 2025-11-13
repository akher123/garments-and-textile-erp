CREATE TABLE [dbo].[Acc_ControlAccounts_Back] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [ParentCode]   NUMERIC (18)   NOT NULL,
    [ControlCode]  NUMERIC (18)   NOT NULL,
    [ControlName]  NVARCHAR (500) NOT NULL,
    [ControlLevel] INT            NULL,
    [SortOrder]    INT            NULL,
    [IsActive]     BIT            NULL
);

