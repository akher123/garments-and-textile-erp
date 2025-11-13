CREATE TABLE [dbo].[OM_CompConsumption] (
    [CompConsumptionId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompId]            VARCHAR (3)    NOT NULL,
    [OrderStyleRefId]   VARCHAR (7)    NOT NULL,
    [ComponentSlNo]     INT            NOT NULL,
    [ConsRefId]         VARCHAR (10)   NOT NULL,
    [EnDate]            DATE           NULL,
    [ComponentRefId]    VARCHAR (3)    NULL,
    [NParts]            INT            NULL,
    [FabricType]        VARCHAR (1)    NOT NULL,
    [Description]       NVARCHAR (250) NULL,
    CONSTRAINT [PK_OM_CompConsumption] PRIMARY KEY CLUSTERED ([CompConsumptionId] ASC)
);

