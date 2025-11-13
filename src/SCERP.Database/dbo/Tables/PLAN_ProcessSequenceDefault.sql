CREATE TABLE [dbo].[PLAN_ProcessSequenceDefault] (
    [ProSecuenceDefaultId] INT         IDENTITY (1, 1) NOT NULL,
    [CompId]               VARCHAR (3) NULL,
    [ProcessRow]           INT         NOT NULL,
    [ProcessRefId]         VARCHAR (3) NOT NULL,
    CONSTRAINT [PK_PLAN_ProcessSequenceDefault] PRIMARY KEY CLUSTERED ([ProSecuenceDefaultId] ASC)
);

