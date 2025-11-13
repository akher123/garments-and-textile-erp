CREATE TABLE [dbo].[OM_Season] (
    [SeasonId]    INT            IDENTITY (1, 1) NOT NULL,
    [CompId]      VARCHAR (3)    NULL,
    [SeasonRefId] VARCHAR (2)    NOT NULL,
    [SeasonName]  NVARCHAR (100) NULL,
    CONSTRAINT [PK_OM_Season] PRIMARY KEY CLUSTERED ([SeasonId] ASC)
);

