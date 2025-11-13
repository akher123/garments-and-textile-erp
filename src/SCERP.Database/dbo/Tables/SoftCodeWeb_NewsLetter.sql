CREATE TABLE [dbo].[SoftCodeWeb_NewsLetter] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [NewsLetterEmailAddress] NVARCHAR (200) NULL,
    [SentReply]              BIT            NULL,
    [ReplyDateTime]          DATETIME       NULL,
    [CreateDateTime]         DATETIME       NULL,
    [IsActive]               BIT            NULL,
    CONSTRAINT [PK_SoftCodeWeb_NewsLetter] PRIMARY KEY CLUSTERED ([Id] ASC)
);

