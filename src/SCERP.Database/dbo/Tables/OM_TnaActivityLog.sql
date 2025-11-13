CREATE TABLE [dbo].[OM_TnaActivityLog] (
    [ActivityLogId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [TnaId]         BIGINT           NOT NULL,
    [EditedDate]    DATETIME         NOT NULL,
    [EditedBy]      UNIQUEIDENTIFIER NOT NULL,
    [ValueText]     VARCHAR (MAX)    NULL,
    [KeyName]       VARCHAR (50)     NULL,
    CONSTRAINT [PK_OM_TnaActivityLog] PRIMARY KEY CLUSTERED ([ActivityLogId] ASC)
);

