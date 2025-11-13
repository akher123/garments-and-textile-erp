CREATE TABLE [dbo].[Acc_GLAccounts_Hidden_Status] (
    [Id]     INT IDENTITY (1, 1) NOT NULL,
    [Status] BIT NULL,
    CONSTRAINT [PK_Acc_GLAccounts_Hidden_Status] PRIMARY KEY CLUSTERED ([Id] ASC)
);

