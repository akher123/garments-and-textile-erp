CREATE TABLE [dbo].[CommPackingCredit] (
    [PackingCreditId] INT              IDENTITY (1, 1) NOT NULL,
    [LcId]            INT              NOT NULL,
    [CreditDate]      DATETIME         NOT NULL,
    [Amount]          FLOAT (53)       NOT NULL,
    [UsdAmount]       FLOAT (53)       NULL,
    [IsAcive]         BIT              NOT NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]     DATETIME         NOT NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    CONSTRAINT [PK_CommPackingCredit_1] PRIMARY KEY CLUSTERED ([PackingCreditId] ASC),
    CONSTRAINT [FK_CommPackingCredit_COMMLcInfo] FOREIGN KEY ([LcId]) REFERENCES [dbo].[COMMLcInfo] ([LcId])
);

