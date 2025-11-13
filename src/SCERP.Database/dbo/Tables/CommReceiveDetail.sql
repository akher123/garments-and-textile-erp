CREATE TABLE [dbo].[CommReceiveDetail] (
    [ReceiveDetailId] INT              IDENTITY (1, 1) NOT NULL,
    [ReceiveId]       INT              NOT NULL,
    [ItemCode]        NVARCHAR (8)     NULL,
    [Quantity]        DECIMAL (18, 5)  NULL,
    [Rate]            DECIMAL (18, 5)  NULL,
    [Value]           DECIMAL (18, 5)  NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_CommReceiveDetail] PRIMARY KEY CLUSTERED ([ReceiveDetailId] ASC),
    CONSTRAINT [FK_CommReceiveDetail_CommReceive] FOREIGN KEY ([ReceiveId]) REFERENCES [dbo].[CommReceive] ([ReceiveId])
);

