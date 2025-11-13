CREATE TABLE [dbo].[PROD_MachingInterruption] (
    [MachingInterruptionId] INT              IDENTITY (1, 1) NOT NULL,
    [MachineId]             INT              NOT NULL,
    [CompId]                VARCHAR (3)      NOT NULL,
    [ProcessRefId]          VARCHAR (3)      NULL,
    [InterrupDate]          DATETIME         NULL,
    [Remarks]               VARCHAR (MAX)    NOT NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_MatchingInterruption] PRIMARY KEY CLUSTERED ([MachingInterruptionId] ASC)
);

