CREATE TABLE [dbo].[PROD_CuttingTag] (
    [CuttingTagId]      BIGINT      IDENTITY (1, 1) NOT NULL,
    [CuttingSequenceId] BIGINT      NOT NULL,
    [CompId]            VARCHAR (3) NOT NULL,
    [ComponentRefId]    VARCHAR (3) NOT NULL,
    [IsSolid]           BIT         NOT NULL,
    [IsPrint]           BIT         NOT NULL,
    [IsEmbroidery]      BIT         NOT NULL,
    CONSTRAINT [PK_PROD_CuttingTag] PRIMARY KEY CLUSTERED ([CuttingTagId] ASC),
    CONSTRAINT [FK_PROD_CuttingTag_PROD_CuttingSequence] FOREIGN KEY ([CuttingSequenceId]) REFERENCES [dbo].[PROD_CuttingSequence] ([CuttingSequenceId])
);

