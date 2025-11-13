CREATE TABLE [dbo].[OM_ActivityResponsible] (
    [ActivityResponsibleId] INT           IDENTITY (1, 1) NOT NULL,
    [CompId]                CHAR (3)      NOT NULL,
    [ActivityId]            INT           NOT NULL,
    [RespobsibleName]       VARCHAR (100) NOT NULL,
    [RespobsiblePhone]      VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_OM_ActivityResponsible] PRIMARY KEY CLUSTERED ([ActivityResponsibleId] ASC)
);

