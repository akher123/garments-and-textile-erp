CREATE TABLE [dbo].[PLAN_Process] (
    [ProcessId]    INT              IDENTITY (1, 1) NOT NULL,
    [CompId]       VARCHAR (3)      NULL,
    [ProcessRefId] VARCHAR (3)      NULL,
    [ProcessCode]  NVARCHAR (10)    NULL,
    [ProcessName]  NVARCHAR (200)   NULL,
    [BufferDay]    INT              NULL,
    [IsRelative]   BIT              NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_PLANProcessList] PRIMARY KEY CLUSTERED ([ProcessId] ASC)
);

