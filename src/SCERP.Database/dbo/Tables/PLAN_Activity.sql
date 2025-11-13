CREATE TABLE [dbo].[PLAN_Activity] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [CompId]       NVARCHAR (3)     NULL,
    [ActivityCode] NVARCHAR (10)    NULL,
    [ActivityName] NVARCHAR (200)   NULL,
    [ActivityMode] NVARCHAR (1)     NULL,
    [StartField]   NVARCHAR (4)     NULL,
    [EndField]     NVARCHAR (4)     NULL,
    [BufferDay]    INT              NULL,
    [IsRelative]   BIT              NULL,
    [SerialId]     INT              NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_PLAN_Activity] PRIMARY KEY CLUSTERED ([Id] ASC)
);

