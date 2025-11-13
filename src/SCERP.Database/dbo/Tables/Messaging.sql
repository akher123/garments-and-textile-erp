CREATE TABLE [dbo].[Messaging] (
    [MessageId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [MessageText] NVARCHAR (MAX)   NULL,
    [SenderId]    UNIQUEIDENTIFIER NULL,
    [ReceiverId]  UNIQUEIDENTIFIER NULL,
    [SendTime]    DATETIME         NULL,
    [IsViewed]    BIGINT           NULL,
    CONSTRAINT [PK_Messageing] PRIMARY KEY CLUSTERED ([MessageId] ASC)
);

