CREATE TABLE [dbo].[StampAmount] (
    [StampAmountId] INT              IDENTITY (1, 1) NOT NULL,
    [Amount]        DECIMAL (18, 2)  NOT NULL,
    [FromDate]      DATETIME         NOT NULL,
    [ToDate]        DATETIME         NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              CONSTRAINT [DF_StampAmount_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_StampAmount] PRIMARY KEY CLUSTERED ([StampAmountId] ASC)
);

