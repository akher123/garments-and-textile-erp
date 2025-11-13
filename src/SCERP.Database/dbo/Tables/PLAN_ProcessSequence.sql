CREATE TABLE [dbo].[PLAN_ProcessSequence] (
    [ProcessSequenceId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [CompId]            VARCHAR (3)  NULL,
    [OrderStyleRefId]   NVARCHAR (7) NULL,
    [ProcessRow]        INT          NOT NULL,
    [ProcessRefId]      VARCHAR (3)  NOT NULL,
    CONSTRAINT [PK_PLAN_ProcessSequence] PRIMARY KEY CLUSTERED ([ProcessSequenceId] ASC)
);

