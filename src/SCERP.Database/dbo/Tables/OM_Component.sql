CREATE TABLE [dbo].[OM_Component] (
    [ComponentId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompId]         VARCHAR (3)    NOT NULL,
    [ComponentRefId] VARCHAR (3)    NOT NULL,
    [ComponentName]  NVARCHAR (100) NOT NULL,
    [Pannel]         VARCHAR (1)    NULL,
    [CompType]       INT            NULL,
    CONSTRAINT [PK_OM_Component] PRIMARY KEY CLUSTERED ([ComponentId] ASC)
);

